namespace Core.Models.Stocks;

public sealed record AddStocksResponse
{
    public required IReadOnlyCollection<AddStockResponse> Stocks { get; init; }
}