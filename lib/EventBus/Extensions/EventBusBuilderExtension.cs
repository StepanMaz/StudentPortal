
using System.Diagnostics.CodeAnalysis;
using StudentPortal.EventBus;

namespace Microsoft.Extensions.DependencyInjection;

public interface IEventBusBuilder
{
    IServiceCollection Services { get; }
}

public static class EventBusBuilderExtension
{
    public static IEventBusBuilder AddSubscription<TEvent, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler>(this IEventBusBuilder eventBusBuilder)
        where TEvent : IntegrationEvent
        where THandler : class, IIntegrationEventHandler<TEvent>
    {
        eventBusBuilder.Services.AddKeyedTransient<IIntegrationEventHandler, THandler>(typeof(TEvent));

        eventBusBuilder.Services.Configure<EventBusSubscriptionInfo>(o =>
        {
            o.EventTypes[typeof(TEvent).Name] = typeof(TEvent);
        });

        return eventBusBuilder;
    }
}