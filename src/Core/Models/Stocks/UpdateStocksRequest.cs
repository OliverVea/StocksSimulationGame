namespace Core.Models.Stocks;

public record UpdateStocksRequest
{
    public required IReadOnlyCollection<UpdateStockRequest> Stocks { get; init; }
}