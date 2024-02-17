using Core.Models.Ids;

namespace Core.Models.Stocks;

public sealed class ListStocksWithIdsRequest
{
    public required IReadOnlyCollection<StockId> StockIds { get; init; }
}