using API.Endpoints.PriceHistory;
using API.Endpoints.Stocks;
using Microsoft.AspNetCore.Builder;

namespace API.Extensions;

internal static class EndpointExtensions
{
    private const string BasePath = "/api";
    
    internal static void AddEndpoints(this WebApplicationBuilder builder)
    {
    }

    internal static void AddEndpoints(this WebApplication app)
    {
        var groupBuilder = app.MapGroup(BasePath);
        
        groupBuilder.AddStockEndpoints();
        groupBuilder.AddPriceHistoryEndpoints();
    }
}