using Microsoft.Extensions.FileProviders;

namespace UI;

internal static class WebApplicationExtensions
{
    internal static WebApplication InstallMiddleware(this WebApplication app)
    {
        app.AddStaticFiles();
        
        app.UseHttpsRedirection();

        app.MapFallbackToFile("public/index.html");
        
        return app;
    }

    private static void AddStaticFiles(this IApplicationBuilder app)
    {
        var options = new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/public")),
        };
        
        app.UseStaticFiles(options);
    }
}