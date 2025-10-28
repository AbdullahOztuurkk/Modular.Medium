using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.EventBus.Abstraction.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds an integration event handler to the service collection.
    /// Handlers are registered as Transient to allow for new instances per event.
    /// </summary>
    /// <typeparam name="TIntegrationEventHandler">The type of the integration event handler.</typeparam>
    /// <param name="services">The IServiceCollection to add the handler to.</param>
    /// <returns>The modified IServiceCollection for chaining.</returns>
    public static IServiceCollection AddIntegrationEventHandler<TIntegrationEventHandler>(this IServiceCollection services)
        where TIntegrationEventHandler : class, IIntegrationEventHandler
    {
        return services.AddTransient<TIntegrationEventHandler>();
    }
}