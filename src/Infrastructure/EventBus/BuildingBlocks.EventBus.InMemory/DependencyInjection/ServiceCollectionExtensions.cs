using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.EventBus.InMemory.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInMemoryEventBus(this IServiceCollection services)
    {
        services.AddSingleton<InMemoryEventBus>();

        services.AddHostedService<InMemoryEventDispatcher>();

        return services;
    }
}