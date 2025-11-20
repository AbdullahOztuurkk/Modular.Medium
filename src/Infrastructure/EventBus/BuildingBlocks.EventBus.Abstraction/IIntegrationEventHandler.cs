namespace BuildingBlocks.EventBus.Abstraction;

/// <summary>
/// Marker interface for all integration event handlers.
/// Used for generic type constraints in DI registration.
/// </summary>
public interface IIntegrationEventHandler { }

/// <summary>
/// Defines the contract for handling a specific integration event.
/// </summary>
/// <typeparam name="TIntegrationEvent">The type of the integration event to handle.</typeparam>
public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event);
}
