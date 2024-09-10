using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using UserManagement.CommandService.Domain.Aggregates;

namespace UserManagement.CommandService.Infrastructure.Handlers
{
    public class EventSourcingHandler : IEventSourcingHandler<UserAggregate>
    {
        private readonly IEventStore _eventStore;
        private readonly IEventProducer _eventProducer;
        
        public EventSourcingHandler(IEventStore eventStore, IEventProducer eventProducer)
        {
            _eventStore = eventStore;
            _eventProducer = eventProducer;
        }

        public async Task<UserAggregate> GetByIdAsync(Guid aggregateId)
        {
            UserAggregate aggregate = new();
            List<BaseEvent>? events = await _eventStore.GetEventsAsync(aggregateId);

            if (events == null || events.Count == 0) 
                return aggregate;

            aggregate.ReplayEvents(events);
            aggregate.Version = events.Select(x => x.Version).Max();

            return aggregate;
        }

        public async Task RepublishEventsAsync()
        {
            List<Guid>? aggregateIds = await _eventStore.GetAggregateIdsAsync();
            
            if (aggregateIds == null || !aggregateIds.Any()) 
                return;

            foreach (Guid aggregateId in aggregateIds)
            {
                List<BaseEvent>? events = await _eventStore.GetEventsAsync(aggregateId);

                foreach (BaseEvent? @event in events)
                {
                    string? topic = Environment.GetEnvironmentVariable("KAFKA TOPIC");
                    await _eventProducer.ProduceAsync(topic, @event);
                }
            }
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }
    }
}