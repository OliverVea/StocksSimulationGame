using Core.Models.Ids;

namespace Core.Models.Asks;

public sealed record GetAsksResponse
{
    public required UserId UserId { get; init; }
    public required IReadOnlyCollection<Ask> Asks { get; init; }
}