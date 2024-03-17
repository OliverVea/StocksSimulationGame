using AutoFixture;
using AutoFixture.Dsl;
using Core.Models;
using Core.Models.Prices;
using Tests.Extensions;

namespace Tests.DataBuilders;

public static partial class DataBuilder
{
    public static IPostprocessComposer<GetStockPriceInIntervalRequest> GetStockPriceInIntervalRequest()
    {
        return Fixture.Build<GetStockPriceInIntervalRequest>()
            .With(x => x.From, SimulationStep(0).Create())
            .With(x => x.To, SimulationStep(int.MaxValue).Create());
    }
    
    public static IPostprocessComposer<GetStockPriceInIntervalResponse> GetStockPricesResponse(IEnumerable<GetStockPriceResponse> stockPrices)
    {
        return GetStockPricesResponse().With(x => x.StockPrices, stockPrices);
    }
    
    public static IPostprocessComposer<GetStockPriceInIntervalResponse> GetStockPricesResponse()
    {
        return Fixture.Build<GetStockPriceInIntervalResponse>();
    }
    
    public static IPostprocessComposer<GetStockPriceResponse> GetStockPriceResponse()
    {
        return Fixture.Build<GetStockPriceResponse>();
    }

    public static IPostprocessComposer<GetStockPricesForStepResponse> GetStockPricesForStepResponse()
    {
        return Fixture.Build<GetStockPricesForStepResponse>()
            .WithEmpty(x => x.StockPrices);
    }

    public static IPostprocessComposer<GetStockPricesForStepResponse> GetStockPricesForStepResponse(
        int? stockPriceCount = null,
        SimulationStep? simulationStep = null)
    {
        var response = GetStockPricesForStepResponse();

        if (stockPriceCount is not null)
        {
            var stockPricesResponse = GetStockPriceResponse();

            if (simulationStep is not null) stockPricesResponse.With(x => x.Step, simulationStep);
            
            var stockPrices = stockPricesResponse.CreateMany(stockPriceCount.Value).ToArray();
            response = response.With(x => x.StockPrices, stockPrices);
        }

        return response;
    }
    
    public static IPostprocessComposer<SetStockPricesRequest> SetStockPricesRequest()
    {
        return Fixture.Build<SetStockPricesRequest>();
    }
    
    public static IPostprocessComposer<SetStockPriceRequest> SetStockPriceRequest()
    {
        return Fixture.Build<SetStockPriceRequest>();
    }
    
    public static IPostprocessComposer<DeleteStockPricesRequest> DeleteStockPricesRequest()
    {
        return Fixture.Build<DeleteStockPricesRequest>();
    }
    
    public static IPostprocessComposer<DeleteStockPriceRequest> DeleteStockPriceRequest()
    {
        return Fixture.Build<DeleteStockPriceRequest>();
    }
    
    public static IPostprocessComposer<DeleteStockPricesResponse> DeleteStockPricesResponse()
    {
        return Fixture.Build<DeleteStockPricesResponse>();
    }
    
    public static IPostprocessComposer<DeleteStockPriceResponse> DeleteStockPriceResponse()
    {
        return Fixture.Build<DeleteStockPriceResponse>();
    }
    
}