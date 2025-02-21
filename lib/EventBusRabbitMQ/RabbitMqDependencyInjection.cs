using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using StudentPortal.EventBus;

namespace StudentPortal.EventBusRabbitMQ;

public static class RabbitMQDependencyInjectionExtension
{
    private const string SectionName = "EventBus";

    public static IEventBusBuilder AddRabbitMQEventBus(this IHostApplicationBuilder builder, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddRabbitMQConnectionFactory(connectionString);

        builder.Services.Configure<EventBusOptions>(builder.Configuration.GetSection(SectionName));

        builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();

        builder.Services.AddSingleton<IHostedService>(sp => (RabbitMQEventBus)sp.GetRequiredService<IEventBus>());

        return new EventBusBuilder(builder.Services);
    }

    public static IHostApplicationBuilder AddRabbitMQConnectionFactory(this IHostApplicationBuilder builder, string connectionString)
    {
        builder.Services.AddSingleton(new ConnectionFactory() { Uri = new Uri(connectionString) });

        return builder;
    }

    private class EventBusBuilder(IServiceCollection services) : IEventBusBuilder
    {
        public IServiceCollection Services => services;
    }
}