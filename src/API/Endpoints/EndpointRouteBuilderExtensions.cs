using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

namespace API.Endpoints;


/// <summary>
/// Provides extension methods for <see cref="IEndpointRouteBuilder" />.
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Adds the stock endpoints to the given <see cref="IEndpointRouteBuilder" />.
    /// </summary>
    public static IEndpointRouteBuilder AddEndpoints(this IEndpointRouteBuilder endpoints, IConfiguration configuration)
    {
        endpoints.AddStockEndpoints();

        return endpoints;
    }
    
}