namespace UserManagement.QueryService.Infrastructure.Handlers;

public class EventHandlerFactory : IEventHandlerFactory
{
    public IEventHandler? GetHandler(Type eventType, IServiceProvider scopeServiceProvider)
    {
        Type handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
        IEventHandler? eventHandler = scopeServiceProvider.GetService(handlerType) as IEventHandler;
        return eventHandler;
    }
}