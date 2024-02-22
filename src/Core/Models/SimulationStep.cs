namespace Core.Models;

public readonly record struct SimulationStep(long Step)
{
    public static SimulationStep operator-(SimulationStep step, long value) => new(step.Step - value);
    public static SimulationStep operator+(SimulationStep step, long value) => new(step.Step + value);
}