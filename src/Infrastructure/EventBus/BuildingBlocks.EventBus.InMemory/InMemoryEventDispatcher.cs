using BuildingBlocks.EventBus.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace BuildingBlocks.EventBus.InMemory;

public sealed class InMemoryEventDispatcher(IServiceProvider sp, InMemoryEventBus bus, ILogger<InMemoryEventDispatcher> logger) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = sp;
    private readonly InMemoryEventBus _eventBus = bus;
    private readonly ILogger<InMemoryEventDispatcher> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("In-memory event dispatcher started.");

        // Start a consumer task for each event type channel
        var consumers = _eventBus.Channels.Select(kvp => ProcessEventsAsync(kvp.Key, kvp.Value.Reader, stoppingToken));

        await Task.WhenAll(consumers);
    }

    private async Task ProcessEventsAsync(string eventTypeName, ChannelReader<IntegrationEvent> reader, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Started processing events for type '{EventTypeName}'.", eventTypeName);

        // Continuously read events from the channel
        await foreach (var @event in reader.ReadAllAsync(cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogDebug("Processing event '{EventTypeName}' (ID: {EventId}).", eventTypeName, @event.CorrelationId);

            //Check if there are any handlers for the event
            if (!_eventBus.Handlers.TryGetValue(eventTypeName, out var handlerTypes) || handlerTypes.Count == 0)
            {
                _logger.LogWarning("No active handlers found for event '{EventTypeName}' (ID: {EventId}).", eventTypeName, @event.CorrelationId);
                continue;
            }

            foreach (var handlerType in handlerTypes.ToList())
            {
                using var scope = _serviceProvider.CreateScope();
                try
                {
                    var handler = scope.ServiceProvider.GetService(handlerType);

                    // If handler is null, log and continue
                    if (handler is null)
                        continue;

                    //Get the IIntegrationEventHandler<T> interface from the handler type
                    var interfaceType = handlerType.GetInterfaces()
                            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>));

                    if (interfaceType is null)
                        continue;

                    // Get the Handle method from the interface
                    var handleMethod = interfaceType.GetMethod("Handle");
                    if (handleMethod is null)
                        continue;

                    // Call the Handle method
                    await (Task)handleMethod.Invoke(handler, new object[] { @event })!;
                }
                catch (Exception ex)
                {
                    // Log the exception but continue processing other handlers
                    _logger.LogError(ex,
                                     "Error handling event '{EventTypeName}' (ID: {EventId}) with handler '{HandlerType}': {ErrorMessage}",
                                     eventTypeName,
                                     @event.CorrelationId,
                                     handlerType.Name,
                                     ex.Message);
                    throw;
                }
            }
        }
    }
}

