using API.Endpoints.SimulationInformation.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints.SimulationInformation;

internal static class EndpointRouteBuilderExtensions
{
    private const string BasePath = "/simulation-information";
    private const string Tag = "Simulation Information";
    
    internal static void AddSimulationInformationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var groupBuilder = endpoints.MapGroup(BasePath);
        
        groupBuilder.WithTags(Tag);
        
        groupBuilder.AddGetSimulationInformation();
    }
}