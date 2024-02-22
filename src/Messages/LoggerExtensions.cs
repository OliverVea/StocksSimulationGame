using Microsoft.Extensions.Logging;

namespace Messages;

public static partial class LoggerExtensions
{
    [LoggerMessage(LogLevel.Information, "Executing simulation step {SimulationStep}")]
    public static partial void SimulationStepped(this ILogger logger, long simulationStep);
}