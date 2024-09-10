using Confluent.Kafka;
using CQRS.Core.Consumers;
using Microsoft.EntityFrameworkCore;
using UserManagement.Common.Config;
using UserManagement.Common.Events;
using UserManagement.QueryService.Domain.Repositories;
using UserManagement.QueryService.Infrastructure.Consumers;
using UserManagement.QueryService.Infrastructure.DataAccess;
using UserManagement.QueryService.Infrastructure.Handlers;
using UserManagement.QueryService.Infrastructure.Repositories;
// ReSharper disable UnusedMethodReturnValue.Local

namespace UserManagement.QueryService.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddInfrastructure(configuration)
            .AddEventHandling()
            .AddApiServices();

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddEventConsumer(configuration);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        void ConfigureDbContext(DbContextOptionsBuilder o) =>
            o.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("SqlServer"));

        services.AddDbContext<DatabaseContext>(ConfigureDbContext);
        services.AddSingleton(new DatabaseContextFactory(ConfigureDbContext));

        using IServiceScope scope = services.BuildServiceProvider().CreateScope();
        DatabaseContext dataContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        dataContext.Database.EnsureCreated();

        return services;
    }

    private static IServiceCollection AddEventHandling(this IServiceCollection services)
    {
        services.AddSingleton<IEventHandlerFactory, EventHandlerFactory>();
        services.AddScoped<IEventHandler<UserCreatedEvent>, UserCreatedEventHandler>();
        services.AddScoped<IEventHandler<UserUpdatedEvent>, UserUpdatedEventHandler>();
        services.AddScoped<IEventHandler<UserDeletedEvent>, UserDeletedEventHandler>();

        return services;
    }

    private static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddHostedService<ConsumerHostedService>();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    private static IServiceCollection AddEventConsumer(this IServiceCollection services, IConfiguration configuration)
    {
        string? messageBroker = configuration.GetValue<string>("MessageBroker");

        switch (messageBroker)
        {
            case "Kafka":
                services.AddScoped<IEventConsumer, KafkaConsumer>();
                services.Configure<ConsumerConfig>(configuration.GetSection(nameof(ConsumerConfig)));
                break;
            case "RabbitMQ":
                services.AddSingleton<IEventConsumer, RabbitMQConsumer>();
                services.Configure<RabbitMQConfig>(configuration.GetSection("RabbitMQConfig"));
                break;
            default:
                throw new ArgumentException($"Unsupported message broker: {messageBroker}", nameof(messageBroker));
        }

        return services;
    }
}