namespace Persistence.Entities;

public sealed class StockPrice : BaseEntity
{
    public required Guid StockId { get; init; }
    public required long SimulationStep { get; init; }
    public required float Price { get; init; }
}