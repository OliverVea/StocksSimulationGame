using Core.Models.Ids;

namespace Core.Models.Prices;

public record SetStockPriceRequest
{
    public required StockId StockId { get; init; }
    public required SimulationStep SimulationStep { get; init; }
    public required float Price { get; init; }
}