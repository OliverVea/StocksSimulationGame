using Core.Models.Ids;

namespace Core.Models.Portfolio;

public sealed record GetUserPortfolioRequest
{
    public required UserId UserId { get; init; }
    public IReadOnlyCollection<StockId>? StockIds { get; init; }
}