using Core.Models.Portfolio;
using OneOf;
using OneOf.Types;

namespace Core.Services;

public interface IUserPortfolioService
{
    Task AddToPortfolioAsync(AddToPortfolioRequest request, CancellationToken cancellationToken);
    Task<OneOf<Success, Error>> RemoveFromPortfolioAsync(RemoveFromPortfolioRequest request, CancellationToken cancellationToken);
    Task<GetUserPortfolioResponse> GetUserPortfolioAsync(GetUserPortfolioRequest request, CancellationToken cancellationToken);
}