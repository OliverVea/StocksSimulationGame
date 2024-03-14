using Aspire.ServiceDefaults;

var builder = DistributedApplication.CreateBuilder(args);

var stocksDatabase = builder.AddSqlServer(Constants.Db)
    .AddDatabase(Constants.StocksDatabase);

var api = builder.AddProject<Projects.Host_API>(Constants.Api)
    .WithEnvironment("Persistence__Provider", "Aspire")
    .WithReference(stocksDatabase);

var ui = builder.AddNpmApp(Constants.Ui, "../../src/frontend", "dev")
    .WithReference(api)
    .WithEndpoint(hostPort: 7067, scheme: "https", env: "PORT")
    .PublishAsDockerFile();

builder.Build().Run();