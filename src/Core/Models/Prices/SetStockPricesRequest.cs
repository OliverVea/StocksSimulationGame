namespace Core.Models.Prices;

public record SetStockPricesRequest
{
    public required IReadOnlyCollection<SetStockPriceRequest> StockPrices { get; init; }
}