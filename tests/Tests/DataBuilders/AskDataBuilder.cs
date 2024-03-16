using AutoFixture.Dsl;
using Core.Models.Asks;
using Core.Models.Ids;

namespace Tests.DataBuilders;

public sealed partial class DataBuilder
{
    public IPostprocessComposer<AskId> AskId()
    {
        return Fixture.Build<AskId>();
    }
    
    public IPostprocessComposer<GetAsksRequest> GetAsksRequest()
    {
        return Fixture.Build<GetAsksRequest>()
            .Without(x => x.AskIds);
    }

    public IPostprocessComposer<GetAsksRequest> GetAsksRequest(UserId userId, IReadOnlyCollection<AskId>? askIds = null)
    {
        var request = GetAsksRequest();

        request = request.With(x => x.UserId, userId);
        if (askIds is not null) request = request.With(x => x.AskIds, askIds);

        return request;
    }

    public IPostprocessComposer<DeleteAsksRequest> DeleteAsksRequest()
    {
        return Fixture.Build<DeleteAsksRequest>();
    }

    public IPostprocessComposer<DeleteAsksRequest> DeleteAsksRequest(UserId userId, IReadOnlyCollection<AskId> askIds)
    {
        var request = DeleteAsksRequest();

        request = request.With(x => x.UserId, userId);
        request = request.With(x => x.AskIds, askIds);

        return request;
    }

    public IPostprocessComposer<CreateAskRequest> CreateAskRequest()
    {
        return Fixture.Build<CreateAskRequest>();
    }

    public IPostprocessComposer<Ask> Ask()
    {
        return Fixture.Build<Ask>();
    }
}