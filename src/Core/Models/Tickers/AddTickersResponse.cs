using Core.Models.Ids;

namespace Core.Models.Tickers;

public sealed record AddTickersResponse()
{
    public required IReadOnlyCollection<TickerId> AddedTickerIds { get; init; }
}