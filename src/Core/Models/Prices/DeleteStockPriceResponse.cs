using Core.Models.Ids;

namespace Core.Models.Prices;

public record DeleteStockPriceResponse
{
    public required StockId StockId { get; init; }
}