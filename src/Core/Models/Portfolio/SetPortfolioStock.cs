using Core.Models.Ids;

namespace Core.Models.Portfolio;

public sealed record SetPortfolioStock
{
    public required StockId StockId { get; init; }
    public required int Quantity { get; init; }
}