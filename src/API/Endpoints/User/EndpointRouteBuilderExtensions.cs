using API.Endpoints.User.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints.User;

internal static class EndpointRouteBuilderExtensions
{
    private const string BasePath = "/user";
    private const string Tag = "User";
    
    internal static void AddUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var groupBuilder = endpoints.MapGroup(BasePath);

        groupBuilder.WithTags(Tag);
        
        groupBuilder.AddGetUser();
        groupBuilder.AddCreateUser();
    }
}

