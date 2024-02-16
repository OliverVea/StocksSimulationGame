using Core.Models.Tickers;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

public static class TickerEndpoints
{
    public static void AddTickerEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/tickers", async (
            [FromServices] ITickerService tickerService) =>
        {
            try
            {
                var tickers = await tickerService.ListTickersAsync(new ListTickersRequest(), CancellationToken.None);
                return Results.Ok(tickers.TickerIds.Select(x => x.Ticker));
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });

        endpoints.MapPost("/api/tickers", async (
            [FromServices] ITickerService tickerService,
            [FromBody] AddTickersRequest request) =>
        {
            try
            {
                var response = await tickerService.AddTickersAsync(request, CancellationToken.None);
                return Results.Ok(response.AddedTickerIds);
            } 
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });

        endpoints.MapDelete("/api/tickers", async (
            [FromServices] ITickerService tickerService,
            [FromBody] DeleteTickersRequest request) =>
        {
            try 
            {
                var response = await tickerService.DeleteTickersAsync(request, CancellationToken.None);
                return Results.Ok(response.DeletedTickerIds);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });
    }
}