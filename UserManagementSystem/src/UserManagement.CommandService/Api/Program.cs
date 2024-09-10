using CQRS.Core.Events;
using MongoDB.Bson.Serialization;
using UserManagement.CommandService.Api;
using UserManagement.Common.Events;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<UserCreatedEvent>();
BsonClassMap.RegisterClassMap<UserUpdatedEvent>();
BsonClassMap.RegisterClassMap<UserDeletedEvent>();

builder.Services.ConfigureServices(builder.Configuration);

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); 

app.Run();