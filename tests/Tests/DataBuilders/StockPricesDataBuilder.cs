using AutoFixture;
using AutoFixture.Dsl;
using Core.Models;
using Core.Models.Ids;
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
        IEnumerable<StockId>? stockIds = null,
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

        if (stockIds is not null)
        {
            var stockPricesResponse = GetStockPriceResponse();

            if (simulationStep is not null) stockPricesResponse.With(x => x.Step, simulationStep);

            var stockPrices = stockIds.Select(id => stockPricesResponse.With(x => x.StockId, id).Create()).ToArray();
            response = response.With(x => x.StockPrices, stockPrices);
        }

        return response;
    }
    
    public static IPostprocessComposer<SetStockPricesRequest> SetStockPricesRequest()
    {
        return Fixture.Build<SetStockPricesRequest>();
    }
    
    public static IPostprocessComposer<SetStockPricesRequest> SetStockPricesRequest(IReadOnlyCollection<SetStockPriceRequest>? stockPrices = null)
    {
        var request = SetStockPricesRequest();
        
        if (stockPrices is not null) request = request.With(x => x.StockPrices, stockPrices);
        
        return request;
    }
    
    public static IPostprocessComposer<SetStockPriceRequest> SetStockPriceRequest()
    {
        return Fixture.Build<SetStockPriceRequest>();
    }
    
    public static IPostprocessComposer<SetStockPriceRequest> SetStockPriceRequest(StockId? stockId = null, SimulationStep? simulationStep = null, Price? price = null)
    {
        var request = SetStockPriceRequest();
        
        if (stockId.HasValue) request = request.With(x => x.StockId, stockId.Value);
        if (simulationStep.HasValue) request = request.With(x => x.SimulationStep, simulationStep.Value);
        if (price.HasValue) request = request.With(x => x.Price, price.Value);
        
        return request;
    }
    
    public static IPostprocessComposer<DeleteStockPricesRequest> DeleteStockPricesRequest()
    {
        return Fixture.Build<DeleteStockPricesRequest>();
    }
    
    public static IPostprocessComposer<DeleteStockPriceRequest> DeleteStockPriceRequest()
    {
        return Fixture.Build<DeleteStockPriceRequest>();
    }

    public static IPostprocessComposer<DeleteStockPriceRequest> DeleteStockPriceRequest(StockId? stockId = null)
    {
        var request = DeleteStockPriceRequest();
        
        if (stockId.HasValue) request = request.With(x => x.StockId, stockId.Value);
        
        return request;
    }
    
    public static IPostprocessComposer<DeleteStockPricesResponse> DeleteStockPricesResponse()
    {
        return Fixture.Build<DeleteStockPricesResponse>();
    }
    
    public static IPostprocessComposer<DeleteStockPriceResponse> DeleteStockPriceResponse()
    {
        return Fixture.Build<DeleteStockPriceResponse>();
    }

    public static IPostprocessComposer<GetStockPricesForStepRequest> GetStockPricesForStepRequest()
    {
        return Fixture.Build<GetStockPricesForStepRequest>()
            .WithEmpty(x => x.StockIds);
    }
    
    public static IPostprocessComposer<GetStockPricesForStepRequest> GetStockPricesForStepRequest(SimulationStep? simulationStep = null, IEnumerable<StockId>? stockIds = null)
    {
        var request = GetStockPricesForStepRequest();
        
        if (simulationStep.HasValue) request = request.With(x => x.SimulationStep, simulationStep.Value);
        if (stockIds is not null) request = request.With(x => x.StockIds, stockIds);
        
        return request;
    }
    
}