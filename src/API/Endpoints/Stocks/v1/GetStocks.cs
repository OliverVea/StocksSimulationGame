using API.Models;
using API.Models.Stocks;
using Core.Models.Prices;
using Core.Models.Stocks;
using Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints.Stocks.v1;

internal static class GetStocks
{
    internal static void AddGetStocks(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet(string.Empty, async (
                [FromServices] IStockService stockService,
                [FromServices] ISimulationInformationService simulationInformationService,
                [FromServices] IStockPriceService stockPriceService,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var stocks = await stockService.ListStocksAsync(new ListStocksRequest(), cancellationToken);
                    var currentStep = await simulationInformationService.GetCurrentSimulationStepAsync(cancellationToken);
                    var request = new GetStockPricesForStepRequest
                    {
                        SimulationStep = currentStep,
                        StockIds = stocks.Stocks.Select(x => x.StockId).ToArray()
                    };
                    var prices = await stockPriceService.GetStockPricesForStepAsync(request, cancellationToken);
                    
                    var stocksWithPrices = stocks.Stocks.Zip(prices.StockPrices, (stock, price) => (stock, price));
                    
                    var response = new ListStocksResponseModel(stocksWithPrices);
                    return Results.Ok(response);
                }
                catch (Exception e)
                {
                    var error = new ErrorModel(e.Message);
                    return Results.BadRequest(error);
                }
            })
            .Produces<ListStocksResponseModel>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .RequireAuthorization(Policies.Admin);
}