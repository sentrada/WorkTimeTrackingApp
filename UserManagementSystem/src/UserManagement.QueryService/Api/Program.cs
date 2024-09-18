using CQRS.Core.Events;
using MongoDB.Bson.Serialization;
using UserManagement.Common.Events;
using UserManagement.QueryService.Api;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins("*")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


builder.Services.ConfigureServices(builder.Configuration);
WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularClient");
app.UseRouting();
app.MapControllers();

app.Run();