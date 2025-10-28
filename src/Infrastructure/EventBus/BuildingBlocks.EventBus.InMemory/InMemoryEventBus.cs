using BuildingBlocks.EventBus.Abstraction;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace BuildingBlocks.EventBus.InMemory;

public sealed class InMemoryEventBus : IEventBus
{
    private readonly ConcurrentDictionary<string, Channel<IntegrationEvent>> _channels = new();
    private readonly ConcurrentDictionary<string, List<Type>> _eventHandlerTypes = new();

    public IReadOnlyDictionary<string, Channel<IntegrationEvent>> Channels => _channels;
    public IReadOnlyDictionary<string, List<Type>> Handlers => _eventHandlerTypes;

    public async Task Publish<TIntegrationEvent>(TIntegrationEvent @event) where TIntegrationEvent : IntegrationEvent
    {
        var eventType = @event.GetType().Name;
        if (_channels.TryGetValue(eventType, out var channel))
            await channel.Writer.WriteAsync(@event);
    }

    public Task Subscribe<TIntegrationEvent, TIntegrationEventHandler>()
        where TIntegrationEvent : IntegrationEvent
        where TIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent>
    {
        var eventType = typeof(TIntegrationEvent).Name;
        var handlerType = typeof(TIntegrationEventHandler);

        var handlers = _eventHandlerTypes.GetOrAdd(eventType, _ => new List<Type>());
        if (!handlers.Contains(handlerType))
            handlers.Add(handlerType);

        _channels.TryAdd(eventType, Channel.CreateUnbounded<IntegrationEvent>());

        return Task.CompletedTask;
    }

    public Task Unsubscribe<TIntegrationEvent, TIntegrationEventHandler>()
        where TIntegrationEvent : IntegrationEvent
        where TIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent>
    {
        var eventType = typeof(TIntegrationEvent).Name;
        var handlerType = typeof(TIntegrationEventHandler);

        if (_eventHandlerTypes.TryGetValue(eventType, out var handlers))
        {
            if (handlers.Contains(handlerType))
                handlers.Remove(handlerType);

            if (handlers.Count == 0)
            {
                _eventHandlerTypes.TryRemove(eventType, out _);
                _channels.TryRemove(eventType, out _);
            }
        }

        return Task.CompletedTask;
    }
}