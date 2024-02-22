namespace Core.Models.Prices;

public class GetStockPricesForStepResponse
{
    public required IReadOnlyCollection<GetStockPriceResponse> StockPrices { get; init; }
}