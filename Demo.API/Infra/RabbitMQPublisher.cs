using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Contracts.Events.Interfaces;
using Demo.Contracts.Queues.Enum;
using RabbitMQ.Client;

namespace Demo.API.Infra
{
    public class RabbitMQPublisher : IEventPublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        public RabbitMQPublisher(IConfiguration configuration)
        {
                ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
                _connection = factory.CreateConnectionAsync().Result;
                _channel = _connection.CreateChannelAsync().Result;

                ConfigureExchange().Wait();
                ConfigureQueues().Wait();
        }

        public async Task ConfigureExchange()
        {
            await _channel.ExchangeDeclareAsync(ExchangeNames.NotificationsExchange, ExchangeType.Direct, durable: true, autoDelete: false);
        }

        public async Task ConfigureQueues()
        {
            await _channel.QueueDeclareAsync(QueueNames.CustomerNotificationQueue, durable: true, exclusive: false, autoDelete: false);
            await _channel.QueueDeclareAsync(QueueNames.AdmNotificationQueue, durable: true, exclusive: false, autoDelete: false);

            await _channel.QueueBindAsync(QueueNames.CustomerNotificationQueue, ExchangeNames.NotificationsExchange, RoutingKeys.CustomerNotification);
            await _channel.QueueBindAsync(QueueNames.AdmNotificationQueue, ExchangeNames.NotificationsExchange, RoutingKeys.AdmNotification);
        }

        public async Task PublishAsync<TEvent>(TEvent @event, string exchangeName, string routingKey) where TEvent : class
        {
            var json = System.Text.Json.JsonSerializer.Serialize(@event);
            byte[] body = System.Text.Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent
            };

            await _channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, mandatory: false, basicProperties: properties, body: body);
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}