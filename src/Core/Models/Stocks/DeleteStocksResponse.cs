using Core.Models.Ids;

namespace Core.Models.Stocks;

public sealed record DeleteStocksResponse
{
    public required IReadOnlyCollection<StockId> DeletedStockIds { get; init; }
};