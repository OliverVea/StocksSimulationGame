using System.Security.Claims;
using API.Extensions;
using Core.Services;

namespace API;

internal class UserIdInitializationService(IUserIdService userIdService)
{
    public void Initialize(ClaimsPrincipal user)
    {
        userIdService.Initialize(user.GetUserId());
    }
}