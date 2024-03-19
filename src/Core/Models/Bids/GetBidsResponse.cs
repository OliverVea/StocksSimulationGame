namespace Core.Models.Bids;

public sealed record GetBidsResponse
{
    public required IReadOnlyCollection<Bid> Bids { get; init; }
}