namespace Persistence.Configuration;

public record PersistenceConfiguration
{
    public const string SectionName = "Persistence";

    public PersistenceProvider? Provider { get; init; }
    public SqliteConfiguration? Sqlite { get; init; }

}