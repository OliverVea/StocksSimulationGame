using Core.Models.Ids;

namespace Core.Models.Tickers;

public sealed class ListTickersWithIdsRequest
{
    public required IReadOnlyCollection<TickerId> TickerIds { get; init; }
}