using Core.Models.Ids;

namespace Core.Models.Bids;

public sealed record DeleteBidsRequest
{
    public required IReadOnlyCollection<BidId> BidIds { get; init; }
}