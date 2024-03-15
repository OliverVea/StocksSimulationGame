namespace Core.Models;

public sealed record SimulationInformation
{
    public required SimulationStep CurrentStep { get; init; }
    public required TimeSpan SimulationStepDuration { get; init; }
    public required DateTime StartTime { get; init; }
}