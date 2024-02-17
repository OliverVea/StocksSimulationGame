using Core.Models.Ids;

namespace Core.Models.Stocks;

public sealed record DeleteStocksRequest
{
    public required IReadOnlySet<StockId> StockIds { get; init; }
    public bool ErrorIfMissing { get; init; } = false;
};