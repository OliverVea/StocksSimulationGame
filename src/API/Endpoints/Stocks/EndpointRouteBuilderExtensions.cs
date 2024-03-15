using API.Endpoints.Stocks.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints.Stocks;

internal static class EndpointRouteBuilderExtensions
{
    private const string BasePath = "/stocks";
    private const string Tag = "Stocks";
    
    internal static void AddStockEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var groupBuilder = endpoints.MapGroup(BasePath);

        groupBuilder.WithTags(Tag);
        
        groupBuilder.AddGetStocks();
        groupBuilder.AddSummarizeStocks();
        groupBuilder.AddAddStocks();
        groupBuilder.AddDeleteStocks();
        groupBuilder.AddUpdateStocks();
    }
}

