using System.Text;
using System.Text.Json;
using CQRS.Core.Events;
using CQRS.Core.Producers;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using UserManagement.CommandService.Infrastructure.Config;
using UserManagement.Common.Config;

namespace UserManagement.CommandService.Infrastructure.Producers;

// ReSharper disable once InconsistentNaming
public class RabbitMQProducer : IEventProducer
{
    private readonly IConnection _connection;

    public RabbitMQProducer(IOptions<RabbitMQConfig> config)
    {
        ConnectionFactory factory = new()
        {
            HostName = config.Value.HostName,
            UserName = config.Value.UserName,
            Password = config.Value.Password
        };
        _connection = factory.CreateConnection();
    }

    public async Task ProduceAsync<T>(string queueName, T @event) where T : BaseEvent
    {
        await Task.Run(() =>
        {
            using IModel? channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            string messageBody = JsonSerializer.Serialize(@event, @event.GetType());
            byte[] body = Encoding.UTF8.GetBytes(messageBody);

            IBasicProperties? properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
        });
    }
}