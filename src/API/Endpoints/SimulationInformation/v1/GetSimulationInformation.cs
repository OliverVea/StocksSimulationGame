using API.Models;
using API.Models.SimulationInformation;
using Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace API.Endpoints.SimulationInformation.v1;

internal static class GetSimulationInformation
{
    internal static void AddGetSimulationInformation(this IEndpointRouteBuilder endpoints) => 
        endpoints.MapGet("{stockId}", async (
                [FromServices] ISimulationInformationService simulationInformationService,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var stockPrices = await simulationInformationService.GetSimulationInformationAsync(cancellationToken);
            
                    var response = new GetSimulationInformationResponseModel(stockPrices);
            
                    return Results.Ok(response);
                }
                catch (Exception e)
                {
                    var error = new ErrorModel(e.Message);
                    return Results.BadRequest(error);
                }
            }).Produces<GetSimulationInformationResponseModel>()
            .Produces<ErrorModel>(StatusCodes.Status400BadRequest);
}