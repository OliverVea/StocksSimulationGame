using System.Security.Claims;
using API.Extensions;
using API.Models;
using API.Models.User;
using Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints.User.v1;

internal static class GetUser
{
    internal static void AddGetUser(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet(string.Empty, async (
                [FromServices] IUserService userService,
                [FromServices] IUserIdService userIdService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var userId = user.GetUserId();
                    userIdService.Initialize(userId);
                    
                    var userInformation = await userService.GetUserAsync(cancellationToken);

                    if (userInformation is null) return Results.NotFound();
                    var response = new GetUserResponseModel(userInformation);

                    return Results.Ok(response);
                }
                catch (Exception e)
                {
                    var error = new ErrorModel(e.Message);
                    return Results.BadRequest(error);
                }
            }).Produces<GetUserResponseModel?>()
            .Produces<ErrorModel>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();
}