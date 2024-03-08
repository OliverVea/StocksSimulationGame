using API.Endpoints.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints;

internal static class StockEndpoints
{
    private const string BasePath = "/api/Stocks";
    
    internal static void AddStockEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var groupBuilder = endpoints.MapGroup(BasePath);

        groupBuilder.WithTags("Stocks");
        
        groupBuilder.AddGetStocks();
        groupBuilder.AddSummarizeStocks();
        groupBuilder.AddAddStocks();
        groupBuilder.AddDeleteStocks();
        groupBuilder.AddUpdateStocks();
    }
}

