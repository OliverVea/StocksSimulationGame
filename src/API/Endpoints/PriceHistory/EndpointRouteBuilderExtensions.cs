using API.Endpoints.PriceHistory.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints.PriceHistory;

internal static class EndpointRouteBuilderExtensions
{
    private const string BasePath = "/price-history";
    private const string Tag = "Price History";
    
    internal static void AddPriceHistoryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var groupBuilder = endpoints.MapGroup(BasePath);
        
        groupBuilder.WithTags(Tag);
        
        groupBuilder.AddGetPriceHistory();
    }
}