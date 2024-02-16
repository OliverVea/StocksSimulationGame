using Core.Models.Ids;

namespace Core.Models.Tickers;

public sealed record AddTickersRequest
{
    public required IReadOnlyCollection<TickerId> TickerIds { get; init; }
    public bool ErrorIfDuplicate { get; init; } = false;
}