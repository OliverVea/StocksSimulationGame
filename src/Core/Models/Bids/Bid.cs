using Core.Models.Ids;
using Core.Models.Prices;

namespace Core.Models.Bids;

public sealed record Bid
{
    public required BidId BidId { get; init; }
    public required UserId UserId { get; init; }
    public required StockId StockId { get; init; }
    public int Amount { get; init; }
    public Price PricePerUnit { get; init; }
}