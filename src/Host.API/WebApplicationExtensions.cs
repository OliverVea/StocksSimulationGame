using API.Extensions;

namespace Host;

internal static class WebApplicationExtensions
{
    internal static WebApplication InstallMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.AddApi();
        
        return app;
    }
}