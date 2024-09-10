using CQRS.Core.Events;

namespace UserManagement.QueryService.Infrastructure.Handlers;

public interface IEventHandler
{
    Task HandleAsync(BaseEvent @event);
}

public interface IEventHandler<in T> : IEventHandler where T : BaseEvent
{
    Task IEventHandler.HandleAsync(BaseEvent @event) => HandleAsync((T)@event);
    Task HandleAsync(T @event);
}