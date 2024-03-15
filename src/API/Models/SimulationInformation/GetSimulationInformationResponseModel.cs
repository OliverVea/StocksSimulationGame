namespace API.Models.SimulationInformation;

/// <summary>
/// Represents the response model for the simulation information endpoint.
/// </summary>
public sealed record GetSimulationInformationResponseModel
{
    /// <summary>
    /// The current step of the simulation.
    /// </summary>
    public long CurrentStep { get; }
    
    /// <summary>
    /// The number of seconds per step in the simulation.
    /// </summary>
    public int SecondsPerStep { get; }
    
    /// <summary>
    /// The start time of the simulation.
    /// </summary>
    public DateTime StartTime { get; }

    internal GetSimulationInformationResponseModel(Core.Models.SimulationInformation simulationInformation)
    {
        CurrentStep = simulationInformation.CurrentStep.Step;
        SecondsPerStep = (int)simulationInformation.SimulationStepDuration.TotalSeconds;
        StartTime = simulationInformation.StartTime;
    }
}