using UI;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureBuilder();

var app = builder.Build();

app.InstallMiddleware();

await app.RunAsync();
