using Core.Models.Ids;

namespace Core.Models.Portfolio;

public sealed record GetUserPortfolioResponse
{
    public required UserId UserId { get; init; }
    public required IReadOnlyCollection<GetUserPortfolioStock> Stocks { get; init; }
}