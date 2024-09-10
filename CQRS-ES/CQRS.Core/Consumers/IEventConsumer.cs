namespace CQRS.Core.Consumers
{
    public interface IEventConsumer
    {
        Task Consume(string source);
        Task StopAsync(CancellationToken cancellationToken);
    }
}