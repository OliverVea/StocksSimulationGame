using Core.Models.Ids;

namespace Core.Models.Stocks;

public sealed record AddStockRequest
{
    public required StockId StockId { get; init; }
    public required string Ticker { get; init; }
    public required float Volatility { get; init; }
    public required float Drift { get; init; }
}