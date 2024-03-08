using API.Models;
using Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints.v1;

internal static class AddStocks
{
    internal static void AddAddStocks(this IEndpointRouteBuilder endpoints)
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
            .Produces<ErrorModel>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .RequireAuthorization(Policies.Admin);
    }
}