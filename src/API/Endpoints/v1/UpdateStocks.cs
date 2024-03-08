using API.Models;
using Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints.v1;

internal static class UpdateStocks
{
    internal static void AddUpdateStocks(this IEndpointRouteBuilder endpoints)
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
            .Produces<ErrorModel>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .RequireAuthorization(Policies.Admin);
    }
}