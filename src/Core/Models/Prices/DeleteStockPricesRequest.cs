namespace Core.Models.Prices;

public record DeleteStockPricesRequest
{
    public required IReadOnlyCollection<DeleteStockPriceRequest> StockPrices { get; init; }
}