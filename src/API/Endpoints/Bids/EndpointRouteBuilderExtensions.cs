using API.Endpoints.Bids.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints.Bids;

internal static class EndpointRouteBuilderExtensions
{
    private const string BasePath = "/bids";
    private const string Tag = "Bids";
    
    internal static void AddBidEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var groupBuilder = endpoints.MapGroup(BasePath);
        
        groupBuilder.WithTags(Tag);
        
        groupBuilder.AddGetBids();
        groupBuilder.AddCreateBid();
        //groupBuilder.AddDeleteBids();
    }
}