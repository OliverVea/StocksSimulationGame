using Host;
using Microsoft.IdentityModel.Logging;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureBuilder();

var app = builder.Build();

app.InstallMiddleware();

IdentityModelEventSource.ShowPII = true;
IdentityModelEventSource.LogCompleteSecurityArtifact = true;

await app.Services.MigrateAsync();

await app.RunAsync();
