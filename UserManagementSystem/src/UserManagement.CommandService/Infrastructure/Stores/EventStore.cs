using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using UserManagement.CommandService.Domain.Aggregates;

namespace UserManagement.CommandService.Infrastructure.Stores
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IEventProducer _eventProducer;

        public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer)
        {
            _eventStoreRepository = eventStoreRepository;
            _eventProducer = eventProducer;
        }

        public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
        {
            List<EventModel>? eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

            if (eventStream == null || eventStream.Count == 0)
                throw new AggregateNotFoundException("Incorrect post ID provided!");

            return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
        }

        public async Task<List<Guid>> GetAggregateIdsAsync()
        {
            List<EventModel>? eventStream = await _eventStoreRepository.FindAllAsync();

            if (eventStream == null || eventStream.Count == 0)
            {
                throw new ArgumentNullException(nameof(eventStream),
                    "Could not retrieve event stream from the event store!");
            }

            return eventStream.Select(x => x.AggregateIdentifier).Distinct().ToList();
        }

        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            List<EventModel>? eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);

            if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
                throw new ConcurrencyException();

            int version = expectedVersion;

            foreach (BaseEvent @event in events)
            {
                version++;
                @event.Version = version;
                string eventType = @event.GetType().Name;
                EventModel eventModel = new()
                {
                    TimeStamp = DateTime.Now,
                    AggregateIdentifier = aggregateId,
                    AggregateType = nameof(UserAggregate),
                    Version = version,
                    EventType = eventType,
                    EventData = @event
                };

                await _eventStoreRepository.SaveAsync(eventModel);

                string? destination = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                await _eventProducer.ProduceAsync(destination, @event);
            }
        }
    }
}