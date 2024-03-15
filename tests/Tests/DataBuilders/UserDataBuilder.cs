using AutoFixture.Dsl;
using Core.Models.Ids;
using Core.Models.User;

namespace Tests.DataBuilders;

public partial class DataBuilder
{
    public IPostprocessComposer<UserId> UserId()
    {
        return Fixture.Build<UserId>();
    }
    
    public IPostprocessComposer<UserId> UserId(string id)
    {
        return UserId().With(x => x.Id, id);
    }

    public IPostprocessComposer<UserInformation> UserInformation()
    {
        return Fixture.Build<UserInformation>();
    }
    
}