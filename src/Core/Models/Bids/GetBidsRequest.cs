using Core.Models.Ids;
using Core.Models.Prices;

namespace Core.Models.Bids;

public sealed record GetBidsRequest
{
    public UserId? UserId { get; init; } = null;
    public IReadOnlyCollection<BidId>? BidIds { get; init; } = null;
    public StockId? StockId { get; init; } = null;
    public Price? MinPrice { get; init; }
}