using API;
using Persistence;
using API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.AddStockEndpoints();

await app.Services.MigrateAsync();

app.Run();