using System.ComponentModel.DataAnnotations;
using API.Models;
using API.Models.PriceHistory;
using Core.Models;
using Core.Models.Ids;
using Core.Models.Prices;
using Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints.PriceHistory.v1;

internal static class GetPriceHistory
{
    internal static void AddGetPriceHistory(this IEndpointRouteBuilder endpoints) => 
        endpoints.MapGet("{stockId}", async (
                [FromServices] IStockPriceService stockPriceService,
                [Required] string stockId, 
                int? dataPoints,
                long? from,
                long? to,
                CancellationToken cancellationToken) =>
        {
            try
            {
                var request = new GetStockPriceInIntervalRequest
                {
                    StockId = new StockId(Guid.Parse(stockId)),
                    From = new SimulationStep(from ?? 0),
                    To = new SimulationStep(to ?? long.MaxValue),
                    DataPoints = dataPoints ?? 100,
                };
            
                var stockPrices = await stockPriceService.GetStockPriceInIntervalAsync(
                    request,
                    cancellationToken);
            
                var response = new GetPriceHistoryResponseModel(stockPrices);
            
                return Results.Ok(response);
            }
            catch (Exception e)
            {
                var error = new ErrorModel(e.Message);
                return Results.BadRequest(error);
            }
        }).Produces<GetPriceHistoryResponseModel>()
        .Produces<ErrorModel>(StatusCodes.Status400BadRequest);
}