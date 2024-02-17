using API.Models;
using Core.Models.Stocks;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

internal static class StockEndpoints
{
    internal static void AddStockEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/Stocks", async (
            [FromServices] IStockService stockService) =>
        {
            try
            {
                var stocks = await stockService.ListStocksAsync(new ListStocksRequest(), CancellationToken.None);
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
            [FromBody] AddStocksRequestModel request) =>
        {
            try
            {
                var stocks = await stockService.AddStocksAsync(request.ToRequest(), CancellationToken.None);
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
            [FromBody] DeleteStocksRequestModel request) =>
        {
            try 
            {
                var deleted = await stockService.DeleteStocksAsync(request.ToRequest(), CancellationToken.None);
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

