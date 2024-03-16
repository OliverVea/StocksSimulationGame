using AutoFixture;
using AutoFixture.Dsl;
using Core.Models.Prices;

namespace Tests.DataBuilders;

public sealed partial class DataBuilder
{
    public IPostprocessComposer<GetStockPriceInIntervalRequest> GetStockPriceInIntervalRequest()
    {
        return Fixture.Build<GetStockPriceInIntervalRequest>()
            .With(x => x.From, SimulationStep(0).Create())
            .With(x => x.To, SimulationStep(int.MaxValue).Create());
    }
    
    public IPostprocessComposer<GetStockPriceInIntervalResponse> GetStockPricesResponse(IEnumerable<GetStockPriceResponse> stockPrices)
    {
        return GetStockPricesResponse().With(x => x.StockPrices, stockPrices);
    }
    
    public IPostprocessComposer<GetStockPriceInIntervalResponse> GetStockPricesResponse()
    {
        return Fixture.Build<GetStockPriceInIntervalResponse>();
    }
    
    public IPostprocessComposer<GetStockPriceResponse> GetStockPriceResponse()
    {
        return Fixture.Build<GetStockPriceResponse>();
    }
    
    public IPostprocessComposer<SetStockPricesRequest> SetStockPricesRequest()
    {
        return Fixture.Build<SetStockPricesRequest>();
    }
    
    public IPostprocessComposer<SetStockPriceRequest> SetStockPriceRequest()
    {
        return Fixture.Build<SetStockPriceRequest>();
    }
    
    public IPostprocessComposer<DeleteStockPricesRequest> DeleteStockPricesRequest()
    {
        return Fixture.Build<DeleteStockPricesRequest>();
    }
    
    public IPostprocessComposer<DeleteStockPriceRequest> DeleteStockPriceRequest()
    {
        return Fixture.Build<DeleteStockPriceRequest>();
    }
    
    public IPostprocessComposer<DeleteStockPricesResponse> DeleteStockPricesResponse()
    {
        return Fixture.Build<DeleteStockPricesResponse>();
    }
    
    public IPostprocessComposer<DeleteStockPriceResponse> DeleteStockPriceResponse()
    {
        return Fixture.Build<DeleteStockPriceResponse>();
    }
    
}