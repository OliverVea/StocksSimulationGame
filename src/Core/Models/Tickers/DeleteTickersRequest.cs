using Core.Models.Ids;

namespace Core.Models.Tickers;

public sealed record DeleteTickersRequest
{
    public required IReadOnlyCollection<TickerId> TickerIds { get; init; }
    public bool ErrorIfNotFound { get; init; } = false;
};