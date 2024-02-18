using API.Models;
using Core.Models.Stocks;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

internal static class StockEndpoints
{
#pragma warning disable MA0051
    internal static void AddStockEndpoints(this IEndpointRouteBuilder endpoints)
#pragma warning restore MA0051
    {
        endpoints.MapGet("/api/Stocks", async (
            [FromServices] IStockService stockService,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var stocks = await stockService.ListStocksAsync(new ListStocksRequest(), cancellationToken);
                var response = new ListStocksResponseModel(stocks);
                return Results.Ok(response);
            }
            catch (Exception e)
            {
                var error = new ErrorModel(e.Message);
                return Results.BadRequest(error);
            }
        })
        .Produces<ListStocksResponseModel>();
        
        endpoints.MapPost("/api/Stocks", async (
            [FromServices] IStockService stockService,
            [FromServices] IStockPriceService stockPriceService,
            [FromBody] AddStocksRequestModel request,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var (addStocksRequest, setStockPricesRequest) = request.ToRequests();
                
                var stocks = await stockService.AddStocksAsync(addStocksRequest, cancellationToken);
                await stockPriceService.SetStockPricesAsync(setStockPricesRequest, cancellationToken);
                
                var response = new AddStocksResponseModel(stocks);
                return Results.Ok(response);
            } 
            catch (Exception e)
            {
                var error = new ErrorModel(e.Message);
                return Results.BadRequest(error);
            }
        })
        .Produces<AddStocksResponseModel>()
        .Produces<ErrorModel>(400);

        endpoints.MapDelete("/api/Stocks", async (
            [FromServices] IStockService stockService,
            [FromBody] DeleteStocksRequestModel request,
            CancellationToken cancellationToken) =>
        {
            try 
            {
                var deleted = await stockService.DeleteStocksAsync(request.ToRequest(), cancellationToken);
                var response = new DeleteStocksResponseModel(deleted);
                return Results.Ok(response);
            }
            catch (Exception e)
            {
                var error = new ErrorModel(e.Message);
                return Results.BadRequest(error);
            }
        })
        .Produces<DeleteStocksResponseModel>()
        .Produces<ErrorModel>(400);
    }
}

