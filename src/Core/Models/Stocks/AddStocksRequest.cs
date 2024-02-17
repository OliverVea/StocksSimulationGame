namespace Core.Models.Stocks;

public sealed record AddStocksRequest
{
    public required IReadOnlyCollection<AddStockRequest> Stocks { get; init; }
    public bool ErrorIfDuplicate { get; init; } = false;
}