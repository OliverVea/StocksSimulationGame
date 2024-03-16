namespace Core.Services;

public sealed class RandomService : IRandomService
{
    private readonly Random _random = new();

    public float SampleNormal(double mean, double standardDeviation)
    {
        var u1 = 1.0 - _random.NextDouble();
        var u2 = 1.0 - _random.NextDouble();
        var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        var randNormal = mean + standardDeviation * randStdNormal;

        return (float) randNormal;
    }
}