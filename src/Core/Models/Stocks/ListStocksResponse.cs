namespace Core.Models.Stocks;

public sealed record ListStocksResponse
{
    public required IReadOnlyCollection<ListStockResponse> Stocks { get; init; }
}