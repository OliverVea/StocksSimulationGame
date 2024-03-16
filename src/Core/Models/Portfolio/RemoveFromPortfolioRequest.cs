using System.Collections.ObjectModel;
using Core.Models.Ids;

namespace Core.Models.Portfolio;

public sealed record RemoveFromPortfolioRequest
{
    public UserId UserId { get; init; }
    public required ReadOnlyCollection<RemoveStockFromPortfolio> Stocks { get; init; }
};