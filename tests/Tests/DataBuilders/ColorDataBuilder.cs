using AutoFixture.Dsl;
using Core.Models;

namespace Tests.DataBuilders;

public static partial class DataBuilder
{
    public static IPostprocessComposer<Color> Color()
    {
        return Fixture.Build<Color>();
    }
}