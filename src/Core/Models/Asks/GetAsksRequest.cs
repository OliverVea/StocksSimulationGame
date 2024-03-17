using Core.Models.Ids;
using Core.Models.Prices;

namespace Core.Models.Asks;

public sealed record GetAsksRequest
{
    public UserId? UserId { get; init; } = null;
    public IReadOnlyCollection<AskId>? AskIds { get; init; } = null;
    public StockId? StockId { get; init; } = null;
    public Price? MinPrice { get; init; } = null;
}