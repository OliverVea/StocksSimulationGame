using Core.Models.User;

namespace Core;

public static class Constants
{
    public static readonly TimeSpan SimulationStepDuration = TimeSpan.FromSeconds(5);
    public static readonly UserBalance StartingBalance = new(1000);
}