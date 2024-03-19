using AutoFixture;
using AutoFixture.Dsl;
using Core.Models.Bids;
using Core.Models.Ids;
using Core.Models.Prices;
using Tests.Extensions;

namespace Tests.DataBuilders;

public static partial class DataBuilder
{
    public static IPostprocessComposer<BidId> BidId()
    {
        return Fixture.Build<BidId>();
    }
    
    public static IPostprocessComposer<GetBidsRequest> GetBidsRequest()
    {
        return Fixture.Build<GetBidsRequest>()
            .Without(x => x.UserId)
            .Without(x => x.StockId)
            .Without(x => x.BidIds)
            .Without(x => x.MinPrice);
    }

    public static IPostprocessComposer<GetBidsRequest> GetBidsRequest(
        UserId? userId = null,
        IReadOnlyCollection<BidId>? bidIds = null,
        StockId? stockId = null,
        Price? minPrice = null)
    {
        var request = GetBidsRequest();

        if (userId is not null) request = request.With(x => x.UserId, userId);
        if (bidIds is not null) request = request.With(x => x.BidIds, bidIds);
        if (stockId is not null) request = request.With(x => x.StockId, stockId);
        if (minPrice is not null) request = request.With(x => x.MinPrice, minPrice);

        return request;
    }

    public static IPostprocessComposer<DeleteBidsRequest> DeleteBidsRequest()
    {
        return Fixture.Build<DeleteBidsRequest>();
    }

    public static IPostprocessComposer<DeleteBidsRequest> DeleteBidsRequest(IReadOnlyCollection<BidId> bidIds)
    {
        var request = DeleteBidsRequest();

        request = request.With(x => x.BidIds, bidIds);

        return request;
    }

    public static IPostprocessComposer<CreateBidRequest> CreateBidRequest()
    {
        return Fixture.Build<CreateBidRequest>();
    }

    public static IPostprocessComposer<Bid> Bid()
    {
        return Fixture.Build<Bid>();
    }

    public static IPostprocessComposer<Bid> Bid(UserId? userId = null)
    {
        var bid = Bid();
        
        if (userId is not null) bid = bid.With(x => x.UserId, userId);
        
        return bid;
    }

    public static IPostprocessComposer<GetBidsResponse> GetBidsResponse()
    {
        return Fixture.Build<GetBidsResponse>()
            .WithEmpty(x => x.Bids);
    }

    public static IPostprocessComposer<GetBidsResponse> GetBidsResponse(IReadOnlyCollection<Bid>? bids = null, int? bidCount = null)
    {
        var response = GetBidsResponse();
        
        if (bidCount is not null) response = response.With(x => x.Bids, Bid().CreateMany(bidCount.Value).ToArray());
        if (bids is not null) response = response.With(x => x.Bids, bids);
        
        return response;
    }
    
    public static IPostprocessComposer<GetBidsResponse> GetBidsResponse(IEnumerable<Bid>? bids)
    {
        var response = GetBidsResponse();
        
        if (bids is not null) response = response.With(x => x.Bids, bids);
        
        return response;
    }
}