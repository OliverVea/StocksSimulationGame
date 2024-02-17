namespace Core.Models.Prices;

public record DeleteStockPricesResponse
{
    public required IReadOnlyCollection<DeleteStockPriceResponse> DeletedStockPrices { get; init; }
}