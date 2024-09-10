using CQRS.Core.Events;

namespace CQRS.Core.Producers
{
    public interface IEventProducer
    {
        Task ProduceAsync<T>(string destination, T @event) where T : BaseEvent;
    }
}