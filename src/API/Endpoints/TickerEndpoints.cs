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
            var tickers = await tickerService.ListTickersAsync(new ListTickersRequest(), CancellationToken.None);
            return Results.Ok(tickers.TickerIds.Select(x => x.Ticker));
        });

        endpoints.MapPost("/api/tickers", async (
            [FromServices] ITickerService tickerService,
            [FromBody] AddTickersRequest request) =>
        {
            var response = await tickerService.AddTickersAsync(request, CancellationToken.None);
            return Results.Ok(response.AddedTickerIds);
        });

        endpoints.MapDelete("/api/tickers", async (
            [FromServices] ITickerService tickerService,
            [FromBody] DeleteTickersRequest request) =>
        {
            var response = await tickerService.DeleteTickersAsync(request, CancellationToken.None);
            return Results.Ok(response.DeletedTickerIds);
        });
    }
}