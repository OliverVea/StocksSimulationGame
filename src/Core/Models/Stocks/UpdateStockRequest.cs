using Core.Models.Ids;

namespace Core.Models.Stocks;

public record UpdateStockRequest
{
    public required StockId StockId { get; init; }
    public string? Ticker { get; init; }
    public float? Volatility { get; init; }
    public float? Drift { get; init; }
}