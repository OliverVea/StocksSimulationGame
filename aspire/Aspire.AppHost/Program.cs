using Aspire.ServiceDefaults;

var builder = DistributedApplication.CreateBuilder(args);

const bool useSqlServer = false;

var api = builder.AddProject<Projects.Host_API>(Constants.Api);

var ui = builder.AddNpmApp(Constants.Ui, "../../src/frontend", "dev")
    .WithReference(api)
    .WithEndpoint(hostPort: 7067, scheme: "https", env: "PORT")
    .PublishAsDockerFile();

if (useSqlServer)
#pragma warning disable CS0162 // Unreachable code detected
{
    var stocksDatabase = builder
        .AddSqlServer(Constants.Db)
        .AddDatabase(Constants.StocksDatabase);

    api.WithEnvironment("Persistence__Provider", "Aspire")
        .WithReference(stocksDatabase);
}
else
{
    api.WithEnvironment("Persistence__Provider", "Sqlite");
}
#pragma warning restore CS0162 // Unreachable code detected



builder.Build().Run();