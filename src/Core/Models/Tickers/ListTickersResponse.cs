using Core.Models.Ids;

namespace Core.Models.Tickers;

public sealed record ListTickersResponse
{
    public required IReadOnlyCollection<TickerId> TickerIds { get; init; }
}