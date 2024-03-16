namespace Core.Models.Prices;

public sealed class GetStockPricesForStepResponse
{
    public required IReadOnlyCollection<GetStockPriceResponse> StockPrices { get; init; }
}