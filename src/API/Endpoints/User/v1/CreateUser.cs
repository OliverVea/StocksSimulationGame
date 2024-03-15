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

internal static class CreateUser
{
    internal static void AddCreateUser(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost(string.Empty, async (
                [FromServices] IUserService userService,
                [FromServices] IUserIdService userIdService,
                ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var userId = user.GetUserId();
                    userIdService.Initialize(userId);
                    
                    var result = await userService.CreateUserAsync(cancellationToken);
                    
                    return result.Match<IResult>(
                        userInformation => Results.Ok(new GetUserResponseModel(userInformation)),
                        _ => Results.NotFound(new ErrorModel("User not found")),
                        _ => Results.Conflict(new ErrorModel("User already exists"))
                    );
                }
                catch (Exception e)
                {
                    var error = new ErrorModel(e.Message);
                    return Results.BadRequest(error);
                }
            }).Produces<GetUserResponseModel>()
            .Produces<ErrorModel>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .RequireAuthorization();
}