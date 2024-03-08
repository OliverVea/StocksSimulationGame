using API.Models;
using Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints.v1;

internal static class DeleteStocks
{
    internal static void AddDeleteStocks(this IEndpointRouteBuilder endpoints)
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
            .Produces<ErrorModel>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .RequireAuthorization(Policies.Admin);
    }
}