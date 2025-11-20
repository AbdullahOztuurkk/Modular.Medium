using BuildingBlocks.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.EventBus.Abstraction.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Configures Event Bus subscriptions for all registered modules that implement <see cref="IModuleEventBusConfigurator"/>.
    /// This method should be called during the application's startup pipeline after services are built.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance representing the running application.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to signal cancellation of the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task ConfigureEventBusSubscriptionsAsync(this WebApplication app, List<IModule> modules,
        CancellationToken cancellationToken = default)
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var eventBus = serviceProvider.GetRequiredService<IEventBus>();

        var eventModules = modules.OfType<IModuleEventBusConfigurator>();

        foreach (var module in eventModules)
        {
            await module.ConfigureModuleSubscriptions(eventBus, cancellationToken);
        }

        app.Lifetime.ApplicationStarted.Register(() =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<WebApplication>>();
            logger.LogInformation("Event Bus subscriptions have been configured and are active.");
        });
    }
}