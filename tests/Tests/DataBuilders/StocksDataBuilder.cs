using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.Dsl;
using Core.Models.Ids;
using Core.Models.Stocks;
using Tests.Extensions;

namespace Tests.DataBuilders;

[SuppressMessage("ReSharper", "MethodOverloadWithOptionalParameter")]
public static partial class DataBuilder
{
    public static IPostprocessComposer<AddStocksRequest> AddStocksRequest()
    {
        return Fixture.Build<AddStocksRequest>()
            .With(x => x.ErrorIfDuplicate, false);
    }
    
    public static IPostprocessComposer<AddStocksRequest> AddStocksRequest(bool errorIfDuplicate = false)
    {
        var request = AddStocksRequest();
        
        if (errorIfDuplicate) request = request.With(x => x.ErrorIfDuplicate, true);
        
        return request;
    }
    
    public static IPostprocessComposer<StockId> StockId()
    {
        return Fixture.Build<StockId>();
    }
    
    public static IPostprocessComposer<AddStockRequest> AddStockRequest()
    {
        return Fixture.Build<AddStockRequest>()
            .With(x => x.StockId, () => new StockId(Guid.NewGuid()))
            .With(x => x.Drift, () => GetRandomFloat(0, 0.1f))
            .With(x => x.Volatility, () => GetRandomFloat(0, 0.1f))
            .With(x => x.Ticker);
    }
    
    public static IPostprocessComposer<AddStocksResponse> AddStocksResponse()
    {
        return Fixture.Build<AddStocksResponse>();
    }

    public static IPostprocessComposer<ListStocksRequest> ListStocksRequest()
    {
        return Fixture.Build<ListStocksRequest>();
    }
    
    public static IPostprocessComposer<ListStocksResponse> ListStocksResponse()
    {
        return Fixture.Build<ListStocksResponse>();
    }
    
    public static IPostprocessComposer<ListStockResponse> ListStockResponse()
    {
        return Fixture.Build<ListStockResponse>()
            .With(x => x.StockId, () => new StockId(Guid.NewGuid()))
            .With(x => x.Drift, () => GetRandomFloat(0, 0.1f))
            .With(x => x.Volatility, () => GetRandomFloat(0, 0.1f))
            .With(x => x.Ticker);
    }

    public static IPostprocessComposer<DeleteStocksRequest> DeleteStocksRequest()
    {
        return Fixture.Build<DeleteStocksRequest>()
            .WithEmpty(x => x.StockIds);
    }

    public static IPostprocessComposer<DeleteStocksRequest> DeleteStocksRequest(IReadOnlySet<StockId>? stockIds = null, int? stockIdCount = null, bool errorIfMissing = false)
    {
        var request = DeleteStocksRequest();
        
        if (stockIdCount != null) request = request.With(x => x.StockIds, StockId().CreateMany(stockIdCount.Value).ToHashSet());
        if (stockIds != null) request = request.With(x => x.StockIds, stockIds);
        if (errorIfMissing) request = request.With(x => x.ErrorIfMissing, true);
        
        return request;
    }
    
    public static IPostprocessComposer<DeleteStocksResponse> DeleteStocksResponse()
    {
        return Fixture.Build<DeleteStocksResponse>();
    }
}