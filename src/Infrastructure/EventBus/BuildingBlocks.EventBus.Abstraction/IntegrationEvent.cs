namespace BuildingBlocks.EventBus.Abstraction;

public record IntegrationEvent 
{
    public Guid CorrelationId { get; } = Guid.NewGuid();
}