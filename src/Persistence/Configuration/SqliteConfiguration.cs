using Microsoft.Data.Sqlite;

namespace Persistence.Configuration;

public record SqliteConfiguration(string ConnectionString)
{
    public SqliteConnection Connection { get; } = new(ConnectionString);
};