using System.Text.Json;
using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using UserManagement.QueryService.Infrastructure.Converters;
using UserManagement.QueryService.Infrastructure.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UserManagement.QueryService.Infrastructure.Consumers;

public class KafkaConsumer : IEventConsumer, IDisposable
{
    private readonly ConsumerConfig _config;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<KafkaConsumer> _logger;
    private readonly IEventHandlerFactory _eventHandlerFactory;
    private IConsumer<string, string>? _consumer;

    private volatile bool _isRunning = true;
    
    public KafkaConsumer(
        IOptions<ConsumerConfig> config,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<KafkaConsumer> logger,
        IEventHandlerFactory eventHandlerFactory)
    {
        _config = config.Value;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _eventHandlerFactory = eventHandlerFactory;
    }

    public async Task Consume(string source)
    {
        _consumer = new ConsumerBuilder<string, string>(_config)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();

        _consumer.Subscribe(source);

        JsonSerializerOptions options = new() { Converters = { new EventJsonConverter() } };
        
        while (_isRunning)
        {
            ConsumeResult<string, string>? consumeResult = _consumer.Consume();

            if (consumeResult?.Message == null) continue;

            string messageValue = consumeResult.Message.Value;
            _logger.LogDebug($"Received message: {messageValue}");

            try
            {
                BaseEvent? @event = JsonSerializer.Deserialize<BaseEvent>(messageValue, options);
                if (@event != null)
                {
                    using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                    {
                        IEventHandler? handler = _eventHandlerFactory.GetHandler(@event.GetType(), scope.ServiceProvider);
                        if (handler != null)
                        {
                            _logger.LogDebug($"Handling event of type: {@event.GetType().Name}");
                            await handler.HandleAsync(@event);
                            _consumer.Commit(consumeResult);
                        }
                        else
                        {
                            _logger.LogWarning($"No handler found for event type: {@event.GetType().Name}");
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to deserialize event");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _isRunning = false;
        return Task.CompletedTask;
    }
    
    public void Dispose()
    {
        _consumer?.Dispose();
    }
}