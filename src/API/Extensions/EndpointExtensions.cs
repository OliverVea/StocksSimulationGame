using API.Endpoints;
using Microsoft.AspNetCore.Builder;

namespace API.Extensions;

internal static class EndpointExtensions
{
    internal static void AddEndpoints(this WebApplicationBuilder builder)
    {
    }

    internal static void AddEndpoints(this WebApplication app)
    {
        app.AddStockEndpoints();
    }
}