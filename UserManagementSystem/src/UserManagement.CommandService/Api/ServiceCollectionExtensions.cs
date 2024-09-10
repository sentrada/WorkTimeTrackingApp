using Confluent.Kafka;
using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using UserManagement.CommandService.Domain.Aggregates;
using UserManagement.CommandService.Infrastructure.Config;
using UserManagement.CommandService.Infrastructure.Handlers;
using UserManagement.CommandService.Infrastructure.Producers;
using UserManagement.CommandService.Infrastructure.Repositories;
using UserManagement.CommandService.Infrastructure.Stores;
using UserManagement.Common.Config;

namespace UserManagement.CommandService.Api;

internal static class ServiceCollectionExtensions
{
    internal static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>()); 

        services.AddControllers();
        services.AddSwaggerGen();
        
        services.Configure<MongoDbConfig>(configuration.GetSection("MongoDbConfig"));
        
        
        
        services.AddScoped<IEventSourcingHandler<UserAggregate>, EventSourcingHandler>();
        services.AddScoped<IEventStore, EventStore>();
        services.AddScoped<IEventStoreRepository, EventStoreRepository>();
        services.AddMessageBroker(configuration);
    }
    
    
    private static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        string? messageBroker = configuration.GetValue<string>("MessageBroker");

        if (messageBroker == "Kafka")
        {
            services.AddScoped<IEventProducer, KafkaProducer>();
            services.Configure<ProducerConfig>(configuration.GetSection("ProducerConfig"));
        }
        else if (messageBroker == "RabbitMQ")
        {
            services.AddSingleton<IEventProducer, RabbitMQProducer>();
            services.Configure<RabbitMQConfig>(configuration.GetSection("RabbitMQConfig"));
        }

        return services;
    }
}