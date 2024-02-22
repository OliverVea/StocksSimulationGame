using API.Models;
using Core.Models.Stocks;
using Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints;

internal static class StockEndpoints
{
    private const string BasePath = "/api/Stocks";
    
    internal static void AddStockEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var groupBuilder = endpoints.MapGroup(BasePath);

        groupBuilder.WithTags("Stocks");
        
        groupBuilder.AddGetStocks();
        groupBuilder.AddAddStocks();
        groupBuilder.AddDeleteStocks();
        groupBuilder.AddUpdateStocks();
    }

    private static void AddGetStocks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(string.Empty, async (
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
    }

    private static void AddAddStocks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(string.Empty, async (
                [FromServices] IStockService stockService,
                [FromServices] IStockPriceService stockPriceService,
                [FromBody] AddStocksRequestModel request,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var addStocksRequest = request.ToRequests();
                    var stocks = await stockService.AddStocksAsync(addStocksRequest, cancellationToken);

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
            .Produces<ErrorModel>(StatusCodes.Status400BadRequest);
    }

    private static void AddDeleteStocks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(string.Empty, async (
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
        .Produces<ErrorModel>(StatusCodes.Status400BadRequest);
    }

    private static void AddUpdateStocks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPatch(string.Empty, async (
            [FromServices] IStockService stockService,
            [FromBody] UpdateStocksRequestModel request,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var updateStocksRequest = request.ToRequest();
                await stockService.UpdateStocksAsync(updateStocksRequest, cancellationToken);
                return Results.Ok();
            }
            catch (Exception e)
            {
                var error = new ErrorModel(e.Message);
                return Results.BadRequest(error);
            }
        }).Produces(200)
        .Produces<ErrorModel>(StatusCodes.Status400BadRequest);
    }
}

