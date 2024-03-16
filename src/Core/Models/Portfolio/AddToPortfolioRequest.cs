using Core.Models.Ids;

namespace Core.Models.Portfolio;

public sealed record AddToPortfolioRequest
{
    public required UserId UserId { get; init; }
    public required IReadOnlyCollection<AddStockToPortfolio> Stocks { get; init; }
};