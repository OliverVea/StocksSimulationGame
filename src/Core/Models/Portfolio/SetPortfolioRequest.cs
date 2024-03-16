using Core.Models.Ids;

namespace Core.Models.Portfolio;

public sealed record SetPortfolioRequest
{
    public required UserId UserId { get; init; }
    public required IReadOnlyCollection<SetPortfolioStock> Stocks { get; init; }
};