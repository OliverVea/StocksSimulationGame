using Core.Models.Ids;

namespace Core.Models.Asks;

public sealed record DeleteAsksRequest
{
    public required UserId UserId { get; init; }
    public required IReadOnlyCollection<AskId> AskIds { get; init; }
}