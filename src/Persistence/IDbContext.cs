using Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

internal interface IDbContext
{
    DbSet<Ticker> Tickers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}