using Core.Models.Ids;

namespace Core.Models.Prices;

public record DeleteStockPriceRequest
{
    public required StockId StockId { get; init; }
}