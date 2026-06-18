using Microsoft.Extensions.Logging;
using TP4_ProgMoviles.Services;

namespace TP4_ProgMoviles
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            // TP4 — FakeStoreAPI: named HttpClient + scoped services.
            // AddHttpClient registers an IHttpClientFactory under the hood;
            // services inject the factory and call CreateClient("FakeStoreApi")
            // per request to avoid socket exhaustion.
            builder.Services.AddHttpClient("FakeStoreApi", c =>
            {
                c.BaseAddress = new Uri("https://fakestoreapi.com/");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
