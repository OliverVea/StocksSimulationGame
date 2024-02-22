using Core.Models.Ids;

namespace Core.Models.Prices;

public record GetStockPriceInIntervalResponse
{
    public required StockId StockId { get; init; }
    public required IReadOnlyCollection<GetStockPriceResponse> StockPrices { get; init; }
}