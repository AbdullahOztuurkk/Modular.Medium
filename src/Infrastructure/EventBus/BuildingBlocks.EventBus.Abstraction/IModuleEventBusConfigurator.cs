namespace BuildingBlocks.EventBus.Abstraction;

/// <summary>
/// Defines a contract for modules to configure their specific
/// Event Bus subscriptions during application startup.
/// </summary>
public interface IModuleEventBusConfigurator
{
    /// <summary>
    /// Configures the Event Bus subscriptions for the module.
    /// This method should be called during the application's startup phase
    /// to register the module's event handlers with the Event Bus.
    /// </summary>
    Task ConfigureModuleSubscriptions(IEventBus eventBus, CancellationToken cancellationToken = default);
}