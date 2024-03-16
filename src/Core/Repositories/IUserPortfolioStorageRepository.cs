using Core.Models.Portfolio;

namespace Core.Repositories;

public interface IUserPortfolioStorageRepository
{
    Task SetPortfolioAsync(SetPortfolioRequest request, CancellationToken cancellationToken);
    Task<GetUserPortfolioResponse> GetUserPortfolioAsync(GetUserPortfolioRequest request, CancellationToken cancellationToken);
}