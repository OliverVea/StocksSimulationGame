using Core.Models.Ids;

namespace Core.Models.Prices;

public record GetStockPricesForStepRequest
{
    public required SimulationStep SimulationStep { get; init; }
    public IReadOnlyCollection<StockId>? StockIds { get; init; }
}