using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Demo.Contracts.Queues.Enum;
using Demo.Consumer.Handlers;
using Demo.Contracts.Events;
using System.Text;
using System.Text.Json;

namespace Demo.Consumer.AdmNotificationWorker
{
    public class AdmNotificationWorker(IConfiguration _configuration, ILogger<AdmNotificationWorker> _logger, ConfirmatedScheduleHandler _confirmatedScheduleHandler) : BackgroundService
    {
        private IChannel _channel = null!;
        private IConnection _connection = null!;

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
            await base.StartAsync(cancellationToken);
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                try
                {
                    var message = JsonSerializer.Deserialize<ConfirmatedScheduleEvent>(json);
                    if(message is not null)
                        await _confirmatedScheduleHandler.HandleAdmScheduleConfirmation(message);

                    await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                } catch (Exception ex)
                {
                    _logger.LogError(ex, "[ADM WORKER] Erro ao processar mensagem. Enviando para DLQ.");
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                }
            };
            
            _channel.BasicConsumeAsync(QueueNames.AdmNotificationQueue, false, consumer);
            return Task.CompletedTask;
        }

        public async override void Dispose()
        {
            await _channel.CloseAsync();
            await _connection.CloseAsync();
        }
    }
}