using System.Security.Claims;
using API.Models;
using API.Models.Bid;
using Core.Models.Bids;
using Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints.Bids.v1;

internal static class GetBids
{
    internal static void AddGetBids(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet(string.Empty, async (
            [FromServices] IBidService bidService,
            [FromServices] UserIdInitializationService userIdInitializationService,
            [FromServices] IUserIdService userIdService,
            ClaimsPrincipal user,
            CancellationToken cancellationToken) =>
        {
            try
            {
                userIdInitializationService.Initialize(user);
                var userId = userIdService.UserId;
                if (userId == null) return Results.Unauthorized();

                var request = new GetBidsRequest
                {
                    UserId = userId
                };

                var bids = await bidService.GetBidsAsync(request, cancellationToken);
                var response = new GetBidsResponseModel(bids);

                return Results.Ok(response);
            }
            catch (Exception e)
            {
                var error = new ErrorModel(e.Message);
                return Results.BadRequest(error);
            }
        }).Produces<GetBidsResponseModel>()
        .Produces<ErrorModel>(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .RequireAuthorization();

}