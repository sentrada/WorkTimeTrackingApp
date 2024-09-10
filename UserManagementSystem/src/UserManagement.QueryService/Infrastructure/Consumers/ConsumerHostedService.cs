using CQRS.Core.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UserManagement.QueryService.Infrastructure.Consumers;

public class ConsumerHostedService : IHostedService
{
    private readonly ILogger<ConsumerHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ConsumerHostedService(ILogger<ConsumerHostedService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event consumer service running.");

        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            IEventConsumer eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();

            string? source = Environment.GetEnvironmentVariable("KAFKA_TOPIC");

            if (!string.IsNullOrEmpty(source))
            {
                Task.Run(() => eventConsumer.Consume(source), cancellationToken);
            }
            else
            {
                _logger.LogError("No topic or queue specified for event consumption.");
            }
        }

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event consumer service stopped.");
        using IServiceScope scope = _serviceProvider.CreateScope();
        IEventConsumer eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
        await eventConsumer.StopAsync(cancellationToken);
    }
}