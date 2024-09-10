using System.Text.Json;
using Confluent.Kafka;
using CQRS.Core.Events;
using CQRS.Core.Producers;
using Microsoft.Extensions.Options;

namespace UserManagement.CommandService.Infrastructure.Producers;

    public class KafkaProducer(IOptions<ProducerConfig> config) : IEventProducer
    {
        private readonly ProducerConfig _config = config.Value;

        public async Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
        {
            using IProducer<string, string>? producer = new ProducerBuilder<string, string>(_config)
                .SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(Serializers.Utf8)
                .Build();

            Message<string, string> eventMessage = new()
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonSerializer.Serialize(@event, @event.GetType())
            };

            DeliveryResult<string, string>? deliveryResult = await producer.ProduceAsync(topic, eventMessage);

            if (deliveryResult.Status == PersistenceStatus.NotPersisted)
            {
                throw new Exception($"Could not produce {@event.GetType().Name} message to topic - {topic} due to the following reason: {deliveryResult.Message}.");
            }
        }
    }