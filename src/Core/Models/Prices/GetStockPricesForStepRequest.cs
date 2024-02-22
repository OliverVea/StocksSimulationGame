using Core.Models.Ids;

namespace Core.Models.Prices;

public record GetStockPricesForStepRequest
{
    public required IReadOnlyCollection<StockId> StockIds { get; init; }
    public required SimulationStep SimulationStep { get; init; }
}