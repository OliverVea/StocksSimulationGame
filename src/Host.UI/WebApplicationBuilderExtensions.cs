namespace UI;

internal static class WebApplicationBuilderExtensions
{
    internal static void ConfigureBuilder(this WebApplicationBuilder app)
    {
        app.Services.AddLogging(c =>
        {
            c.AddConsole();
        });
        
        
    }

}