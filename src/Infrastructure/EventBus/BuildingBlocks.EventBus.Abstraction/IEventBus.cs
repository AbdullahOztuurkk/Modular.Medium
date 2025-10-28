namespace BuildingBlocks.EventBus.Abstraction;

/// <summary>
/// Defines the contract for an Event Bus, allowing publishing and subscribing to integration events.
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publishes an integration event to the bus.
    /// </summary>
    /// <typeparam name="TIntegrationEvent">The type of the integration event.</typeparam>
    /// <param name="event">The integration event instance to publish.</param>
    Task Publish<TIntegrationEvent>(TIntegrationEvent @event)
        where TIntegrationEvent : IntegrationEvent;

    /// <summary>
    /// Subscribes a specific event handler to a particular integration event type.
    /// </summary>
    /// <typeparam name="TIntegrationEvent">The type of the integration event to subscribe to.</typeparam>
    /// <typeparam name="TIntegrationEventHandler">The type of the handler that will process the event.</typeparam>
    Task Subscribe<TIntegrationEvent, TIntegrationEventHandler>()
        where TIntegrationEvent : IntegrationEvent
        where TIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent>;

    /// <summary>
    /// Unsubscribes a specific event handler from a particular integration event type.
    /// </summary>
    /// <typeparam name="TIntegrationEvent">The type of the integration event to unsubscribe from.</typeparam>
    /// <typeparam name="TIntegrationEventHandler">The type of the handler to unsubscribe.</typeparam>
    Task Unsubscribe<TIntegrationEvent, TIntegrationEventHandler>()
        where TIntegrationEvent : IntegrationEvent
        where TIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent>;
}