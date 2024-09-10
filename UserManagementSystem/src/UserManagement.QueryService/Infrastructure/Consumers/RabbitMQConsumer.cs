using System.Text;
using System.Text.Json;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UserManagement.Common.Config;
using UserManagement.QueryService.Infrastructure.Converters;
using UserManagement.QueryService.Infrastructure.Handlers;
// ReSharper disable InconsistentNaming

namespace UserManagement.QueryService.Infrastructure.Consumers;
public class RabbitMQConsumer : IEventConsumer, IDisposable
{
    private readonly IConnection _connection;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<RabbitMQConsumer> _logger;
    private readonly IEventHandlerFactory _eventHandlerFactory;
    private readonly IModel _channel;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public RabbitMQConsumer(
        IOptions<RabbitMQConfig> config,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<RabbitMQConsumer> logger,
        IEventHandlerFactory eventHandlerFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _eventHandlerFactory = eventHandlerFactory;
        _cancellationTokenSource = new CancellationTokenSource();

        ConnectionFactory factory = new()
        {
            HostName = config.Value.HostName,
            UserName = config.Value.UserName,
            Password = config.Value.Password,
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public async Task Consume(string source)
    {
        _channel.QueueDeclare(queue: source, durable: true, exclusive: false, autoDelete: false, arguments: null);

        AsyncEventingBasicConsumer consumer = new(_channel);
        consumer.Received += ProcessMessageAsync;

        _channel.BasicConsume(queue: source, autoAck: false, consumer: consumer);

        _logger.LogInformation($"Started consuming messages from queue: {source}");
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            // RabbitMQ Consumer is event-driven, so this while loop acts like a listener
            // We keep it running until cancellation is requested.
            await Task.Delay(1000, _cancellationTokenSource.Token);
        }
    }

    private async Task ProcessMessageAsync(object sender, BasicDeliverEventArgs ea)
    {
        try
        {
            byte[] body = ea.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);

            _logger.LogDebug($"Received message: {message}");

            BaseEvent? @event = JsonSerializer.Deserialize<BaseEvent>(message,
                new JsonSerializerOptions { Converters = { new EventJsonConverter() } });

            if (@event != null)
            {
                using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                {
                    IEventHandler? handler = _eventHandlerFactory.GetHandler(@event.GetType(),scope.ServiceProvider);
                    if (handler != null)
                    {
                        await handler.HandleAsync(@event);
                        _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    else
                    {
                        _logger.LogWarning($"No handler found for event type: {@event.GetType().Name}");
                        _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
                    }
                }
            }
            else
            {
                _logger.LogWarning("Failed to deserialize event");
                _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message");
            _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
        }
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask; 
    }
}