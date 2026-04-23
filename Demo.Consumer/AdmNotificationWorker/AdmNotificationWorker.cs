using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Demo.Contracts.Queues.Enum;
using Demo.Consumer.Handlers;
using Demo.Contracts.Events;
using System.Text;
using System.Text.Json;
namespace Demo.Consumer.AdmNotificationWorker
{
    public class AdmNotificationWorker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdmNotificationWorker> _logger;
        private IChannel _channel = null!;
        private IConnection _connection = null!;
        private readonly IServiceProvider _serviceProvider;

        public AdmNotificationWorker(IConfiguration _configuration, ILogger<AdmNotificationWorker> _logger, IServiceProvider serviceProvider)
        {
            this._configuration = _configuration;
            this._logger = _logger;
            this._serviceProvider = serviceProvider;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
                Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
                UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest"
            };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(
                queue: QueueNames.AdmNotificationQueue,
                durable: true,
                exclusive: false,
                autoDelete: false
            );
            await base.StartAsync(cancellationToken);
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                try
                {
                    using(var scope = _serviceProvider.CreateScope())
                    {
                        var _confirmatedScheduleHandler = scope.ServiceProvider.GetRequiredService<ConfirmatedScheduleHandler>();
                        var _canceledScheduleHandler = scope.ServiceProvider.GetRequiredService<CanceledScheduleHandler>();

                        var message = JsonSerializer.Deserialize<ConfirmatedScheduleEvent>(json);
                        if(message is not null)
                            await _confirmatedScheduleHandler.HandleAdmScheduleConfirmation(message);
                        else 
                        {
                            var canceledMessage = JsonSerializer.Deserialize<CanceledScheduleEvent>(json);
                            if(canceledMessage is not null)
                                await _canceledScheduleHandler.HandleAdmScheduleCancellation(canceledMessage);
                            else
                                _logger.LogWarning("[ADM WORKER] Mensagem recebida com formato desconhecido: {Json}", json);
                        }

                        await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                    }
                } catch (Exception ex)
                {
                    _logger.LogError(ex, "[ADM WORKER] Erro ao processar mensagem. Enviando para DLQ.");
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                }
            };
            
            await _channel.BasicConsumeAsync(QueueNames.AdmNotificationQueue, false, consumer);
        }

        public async override void Dispose()
        {
            await _channel.CloseAsync();
            await _connection.CloseAsync();
        }
    }
}