using Aimrank.Agones.Core.Events;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System;

namespace Aimrank.Agones.Infrastructure.EventBus
{
    internal class RabbitMQEventsDispatcher : IEventsDispatcher, IDisposable
    {
        private readonly RabbitMQSettings _rabbitMqSettings;
        private readonly IBasicProperties _basicProperties;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQEventsDispatcher(IOptions<RabbitMQSettings> rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings.Value;

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSettings.HostName,
                Port = _rabbitMqSettings.Port,
                UserName = _rabbitMqSettings.Username,
                Password = _rabbitMqSettings.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_rabbitMqSettings.ExchangeName, "direct", true, false, null);
            _basicProperties = _channel.CreateBasicProperties();
            _basicProperties.Persistent = true;
        }

        public void Dispatch(IEvent @event)
            => _channel.BasicPublish(
                _rabbitMqSettings.ExchangeName,
                GetRoutingKey(@event),
                _basicProperties,
                GetEventBody(@event));

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Dispose();
        }

        private string GetRoutingKey(IEvent @event) => $"{_rabbitMqSettings.ServiceName}.{@event.GetType().Name}";

        private byte[] GetEventBody(IEvent @event)
        {
            var text = JsonSerializer.Serialize(@event, @event.GetType());
            return Encoding.UTF8.GetBytes(text);
        }
    }
}