namespace UserManagement.QueryService.Infrastructure.Handlers;

public interface IEventHandlerFactory
{
    IEventHandler? GetHandler(Type eventType, IServiceProvider scopeServiceProvider);
}