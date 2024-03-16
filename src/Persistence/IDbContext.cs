using Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

internal interface IDbContext
{
    DbSet<Stock> Stocks { get; }
    DbSet<StockPrice> StockPrices { get; }
    DbSet<CurrentSimulationStep> CurrentSimulationSteps { get; }
    DbSet<UserInformation> UserInformation { get; }
    DbSet<Ask> Asks { get; }
    DbSet<UserPortfolioStock> UserPortfolioStocks { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}