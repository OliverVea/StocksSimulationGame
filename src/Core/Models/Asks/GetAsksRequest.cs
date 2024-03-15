using Core.Models.Ids;

namespace Core.Models.Asks;

public sealed record GetAsksRequest
{
    public required UserId UserId { get; init; }
    public IReadOnlyCollection<AskId>? AskIds { get; init; } = null;
}