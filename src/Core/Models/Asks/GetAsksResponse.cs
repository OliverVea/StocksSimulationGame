namespace Core.Models.Asks;

public sealed record GetAsksResponse
{
    public required IReadOnlyCollection<Ask> Asks { get; init; }
}