using System.Security.Claims;
using API.Models;
using API.Models.Bid;
using Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints.Bids.v1;

internal static class CreateBid
{
    internal static void AddCreateBid(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost(string.Empty, async (
                [FromServices] IBidService bidService,
                [FromServices] UserIdInitializationService userIdInitializationService,
                [FromServices] IUserIdService userIdService,
                [FromServices] IUserService userService,
                [FromBody] CreateBidRequestModel model,
                ClaimsPrincipal claimsPrincipal,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    userIdInitializationService.Initialize(claimsPrincipal);
                    
                    var user = await userService.GetUserAsync(cancellationToken);
                    if (user is null) return Results.NotFound("User not found.");

                    var request = model.ToRequest(user.UserId);
                    var bid = await bidService.CreateBidAsync(request, cancellationToken);
                    var response = new CreateBidResponseModel(bid);

                    return Results.Ok(response);
                }
                catch (Exception e)
                {
                    var error = new ErrorModel(e.Message);
                    return Results.BadRequest(error);
                }
            }).Produces<CreateBidResponseModel>()
            .Produces<ErrorModel>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();

}