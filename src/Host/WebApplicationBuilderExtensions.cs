using API.Endpoints;

namespace Host;

internal static class WebApplicationBuilderExtensions
{
    internal static WebApplication InstallMiddleware(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.AddEndpoints(app.Configuration);
        
        return app;
    }
}