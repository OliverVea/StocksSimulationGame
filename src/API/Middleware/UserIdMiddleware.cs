using API.Extensions;
using Core.Services;
using Microsoft.AspNetCore.Http;

namespace API.MiddleWare;

[Obsolete("IUserIdService is recreated in request scope, undoing the initialization in the middleware")]
internal class UserIdMiddleware(RequestDelegate next, IUserIdService userIdService)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var id = context.User.GetUserId();
        userIdService.Initialize(id);

        await _next(context);
    }
}