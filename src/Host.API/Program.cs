using Host;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureBuilder(builder.Configuration);

var app = builder.Build();

app.InstallMiddleware();

await app.Services.MigrateAsync();

await app.RunAsync();
