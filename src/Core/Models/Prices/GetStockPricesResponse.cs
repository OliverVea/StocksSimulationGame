namespace Core.Models.Prices;

public record GetStockPricesResponse
{
    public required IReadOnlyCollection<GetStockPriceResponse> StockPrices { get; init; }
}