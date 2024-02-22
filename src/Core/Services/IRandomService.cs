namespace Core.Services;

public interface IRandomService
{
    public float SampleNormal(double mean, double standardDeviation);
}