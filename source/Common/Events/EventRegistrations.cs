using Microsoft.Extensions.DependencyInjection;

namespace Common.Events;

public static class EventRegistrations
{
    public static IServiceCollection AddEvent<TEvent, TEventData>(this IServiceCollection services)
        where TEvent : class, IEventSource<TEventData>
    {
        services
            .AddSingleton<IEventSource<TEventData>, TEvent>()
            .AddSingleton<IEvent<TEventData>, TEvent>();

        return services;
    }
}