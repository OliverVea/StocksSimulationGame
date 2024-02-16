using Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

internal class ProjectDbContext(DbContextOptions<ProjectDbContext> options) : DbContext(options), IDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticker>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TickerId).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }

    public required DbSet<Ticker> Tickers { get; init; }
}