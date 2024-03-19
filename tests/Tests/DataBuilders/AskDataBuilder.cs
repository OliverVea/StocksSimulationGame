using AutoFixture;
using AutoFixture.Dsl;
using Core.Models.Asks;
using Core.Models.Ids;
using Core.Models.Prices;
using Tests.Extensions;

namespace Tests.DataBuilders;

public static partial class DataBuilder
{
    public static IPostprocessComposer<AskId> AskId()
    {
        return Fixture.Build<AskId>();
    }
    
    public static IPostprocessComposer<GetAsksRequest> GetAsksRequest()
    {
        return Fixture.Build<GetAsksRequest>()
            .Without(x => x.UserId)
            .Without(x => x.StockId)
            .Without(x => x.AskIds)
            .Without(x => x.MaxPrice);
    }

    public static IPostprocessComposer<GetAsksRequest> GetAsksRequest(
        UserId? userId = null,
        IReadOnlyCollection<AskId>? askIds = null,
        StockId? stockId = null,
        Price? maxPrice = null)
    {
        var request = GetAsksRequest();

        if (userId is not null) request = request.With(x => x.UserId, userId);
        if (askIds is not null) request = request.With(x => x.AskIds, askIds);
        if (stockId is not null) request = request.With(x => x.StockId, stockId);
        if (maxPrice is not null) request = request.With(x => x.MaxPrice, maxPrice);

        return request;
    }

    public static IPostprocessComposer<DeleteAsksRequest> DeleteAsksRequest()
    {
        return Fixture.Build<DeleteAsksRequest>();
    }

    public static IPostprocessComposer<DeleteAsksRequest> DeleteAsksRequest(IReadOnlyCollection<AskId> askIds)
    {
        var request = DeleteAsksRequest();

        request = request.With(x => x.AskIds, askIds);

        return request;
    }

    public static IPostprocessComposer<CreateAskRequest> CreateAskRequest()
    {
        return Fixture.Build<CreateAskRequest>();
    }

    public static IPostprocessComposer<Ask> Ask()
    {
        return Fixture.Build<Ask>();
    }

    public static IPostprocessComposer<Ask> Ask(UserId? userId = null)
    {
        var ask = Ask();
        
        if (userId is not null) ask = ask.With(x => x.UserId, userId);
        
        return ask;
    }

    public static IPostprocessComposer<GetAsksResponse> GetAsksResponse()
    {
        return Fixture.Build<GetAsksResponse>()
            .WithEmpty(x => x.Asks);
    }

    public static IPostprocessComposer<GetAsksResponse> GetAsksResponse(IReadOnlyCollection<Ask>? asks = null, int? askCount = null)
    {
        var response = GetAsksResponse();
        
        if (askCount is not null) response = response.With(x => x.Asks, Ask().CreateMany(askCount.Value).ToArray());
        if (asks is not null) response = response.With(x => x.Asks, asks);
        
        return response;
    }
    
    public static IPostprocessComposer<GetAsksResponse> GetAsksResponse(IEnumerable<Ask>? asks)
    {
        var response = GetAsksResponse();
        
        if (asks is not null) response = response.With(x => x.Asks, asks);
        
        return response;
    }
}