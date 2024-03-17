using AutoFixture.Dsl;
using Core.Models.Ids;
using Core.Models.User;

namespace Tests.DataBuilders;

public static partial class DataBuilder
{
    public static IPostprocessComposer<UserId> UserId()
    {
        return Fixture.Build<UserId>();
    }
    
    public static IPostprocessComposer<UserId> UserId(string id)
    {
        return UserId().With(x => x.Id, id);
    }

    public static IPostprocessComposer<UserInformation> UserInformation()
    {
        return Fixture.Build<UserInformation>();
    }
    
}