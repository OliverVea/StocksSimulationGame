using AutoFixture;

namespace Tests.DataBuilders;

public partial class DataBuilder
{
    private static readonly Fixture Fixture = new();
    private static readonly Random Random = new();

    private static float GetRandomFloat(float from = 0, float to = 1)
    {
        return (float)Random.NextDouble() * (to - from) + from;
    }
}