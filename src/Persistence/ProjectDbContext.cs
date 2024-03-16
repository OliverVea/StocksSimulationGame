using Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

internal sealed class ProjectDbContext(DbContextOptions<ProjectDbContext> options) : DbContext(options), IDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasIndex(e => e.StockId).IsUnique();
            entity.HasIndex(e => e.Ticker).IsUnique();
        });
        
        modelBuilder.Entity<StockPrice>(entity =>
        {
            entity.HasAlternateKey(e => new { e.StockId, e.SimulationStep });
            entity.HasIndex(e => e.StockId);
            entity.HasIndex(e => e.SimulationStep);
        });

        base.OnModelCreating(modelBuilder);
    }

    public required DbSet<Stock> Stocks { get; init; }
    public required DbSet<StockPrice> StockPrices { get; init; }
    public required DbSet<CurrentSimulationStep> CurrentSimulationSteps { get; init; }
    public required DbSet<UserInformation> UserInformation { get; init; }
    public required DbSet<Ask> Asks { get; init; }
    public required DbSet<UserPortfolioStock> UserPortfolioStocks { get; init; }
}