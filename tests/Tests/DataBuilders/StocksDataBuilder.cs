using AutoFixture;
using AutoFixture.Dsl;
using Core.Models.Ids;
using Core.Models.Stocks;

namespace Tests.DataBuilders;

public sealed partial class DataBuilder
{
    public IPostprocessComposer<AddStocksRequest> AddStocksRequest()
    {
        return Fixture.Build<AddStocksRequest>();
    }
    
    public IPostprocessComposer<StockId> StockId()
    {
        return Fixture.Build<StockId>();
    }
    
    public IPostprocessComposer<AddStockRequest> AddStockRequest()
    {
        return Fixture.Build<AddStockRequest>()
            .With(x => x.StockId, () => new StockId(Guid.NewGuid()))
            .With(x => x.Drift, () => GetRandomFloat(0, 0.1f))
            .With(x => x.Volatility, () => GetRandomFloat(0, 0.1f))
            .With(x => x.Ticker);
    }
    
    public IPostprocessComposer<AddStocksResponse> AddStocksResponse()
    {
        return Fixture.Build<AddStocksResponse>();
    }

    public IPostprocessComposer<ListStocksRequest> ListStocksRequest()
    {
        return Fixture.Build<ListStocksRequest>();
    }
    
    public IPostprocessComposer<ListStocksResponse> ListStocksResponse()
    {
        return Fixture.Build<ListStocksResponse>();
    }
    
    public IPostprocessComposer<ListStockResponse> ListStockResponse()
    {
        return Fixture.Build<ListStockResponse>()
            .With(x => x.StockId, () => new StockId(Guid.NewGuid()))
            .With(x => x.Drift, () => GetRandomFloat(0, 0.1f))
            .With(x => x.Volatility, () => GetRandomFloat(0, 0.1f))
            .With(x => x.Ticker);
    }

    public IPostprocessComposer<DeleteStocksRequest> DeleteStocksRequest()
    {
        return Fixture.Build<DeleteStocksRequest>()
            .With(x => x.StockIds, () => StockId().CreateMany(3).ToHashSet());
    }
    
    public IPostprocessComposer<DeleteStocksResponse> DeleteStocksResponse()
    {
        return Fixture.Build<DeleteStocksResponse>();
    }
}