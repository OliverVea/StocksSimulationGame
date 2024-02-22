using Core.Models.Ids;

namespace Core.Models.Prices;

public record GetStockPriceResponse
{
    public required StockId StockId { get; init; }
    public required SimulationStep Step { get; init; }
    public required Price Price { get; init; }
}