using Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

internal interface IDbContext
{
    DbSet<Stock> Stocks { get; }
    DbSet<StockPrice> StockPrices { get; }
    DbSet<CurrentSimulationStep> CurrentSimulationSteps { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}