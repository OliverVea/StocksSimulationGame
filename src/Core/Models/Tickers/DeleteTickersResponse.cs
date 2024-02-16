using Core.Models.Ids;

namespace Core.Models.Tickers;

public sealed record DeleteTickersResponse
{
    public required IReadOnlyCollection<TickerId> DeletedTickerIds { get; init; }
};