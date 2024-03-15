using Core.Models.Asks;
using Core.Models.Ids;
using Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Core.Services;

public class AskService(ILogger<AskService> logger, IAskStorageRepository storageRepository) : IAskService
{
    private static AskId NewAskId() => new(Guid.NewGuid());

    public async Task<CreateAskResponse> CreateAskAsync(CreateAskRequest request, CancellationToken cancellationToken)
    {
        var askId = NewAskId();
        await storageRepository.CreateAskAsync(askId, request, cancellationToken);
        
        return new CreateAskResponse
        {
            AskId = askId,
            UserId = request.UserId,
            StockId = request.StockId,
            Amount = request.Amount,
            PricePerUnit = request.PricePerUnit
        };
    }

    public Task<GetAsksResponse> GetAsksAsync(GetAsksRequest request, CancellationToken cancellationToken)
    {
        return storageRepository.GetAsksAsync(request, cancellationToken);
    }

    public async Task<DeleteAsksResponse> DeleteAsksAsync(DeleteAsksRequest request, CancellationToken cancellationToken)
    {
        var existingRequest = new GetAsksRequest { UserId = request.UserId, AskIds = request.AskIds };
        var existingAsks = await storageRepository.GetAsksAsync(existingRequest, cancellationToken);
        
        var missing = request.AskIds.Except(existingAsks.Asks.Select(a => a.AskId)).ToArray();
        
        if (missing.Any())
        {
            logger.LogWarning("User {UserId} tried to delete Asks with the following ids {MissingAsks}", request.UserId, missing);
        }

        request = request with
        {
            AskIds = request.AskIds.Except(missing).ToArray()
        };
        
        await storageRepository.DeleteAsksAsync(request, cancellationToken);

        return new DeleteAsksResponse();
    }
}