using Core.Models.Asks;
using Core.Models.Ids;
using Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Core.Services;

public sealed class AskService(ILogger<AskService> logger, IAskStorageRepository storageRepository) : IAskService
{
    private static AskId NewAskId() => new(Guid.NewGuid());

    public async Task<Ask> CreateAskAsync(CreateAskRequest request, CancellationToken cancellationToken)
    {
        var ask = new Ask
        {
            AskId = NewAskId(),
            StockId = request.StockId,
            UserId = request.UserId,
            PricePerUnit = request.PricePerUnit,
            Amount = request.Amount,
        };
        
        await storageRepository.CreateAskAsync(ask, cancellationToken);

        return ask;
    }

    public Task<GetAsksResponse> GetAsksAsync(GetAsksRequest request, CancellationToken cancellationToken)
    {
        return storageRepository.GetAsksAsync(request, cancellationToken);
    }

    public async Task DeleteAsksAsync(DeleteAsksRequest request, CancellationToken cancellationToken)
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
    }
}