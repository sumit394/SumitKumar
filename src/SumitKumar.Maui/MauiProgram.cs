using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SumitKumar.Application;
using SumitKumar.Shared.Services;
using System.Reflection;

namespace SumitKumar.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // Bundled appsettings.json from MauiAssets
        var stream = typeof(MauiProgram).Assembly
            .GetManifestResourceStream("SumitKumar.Maui.appsettings.json");
        if (stream is not null)
        {
            builder.Configuration.AddJsonStream(stream);
        }

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        var apiBase = builder.Configuration["Api:BaseAddress"] ?? "https://localhost:5001/";

        builder.Services.AddHttpClient<PortfolioApiClient>(c =>
            c.BaseAddress = new Uri(apiBase));

        // Bind the same PortfolioOptions used by the web project so the public
        // Razor components render identically on mobile.
        builder.Services.Configure<SumitKumar.Application.Options.PortfolioOptions>(
            builder.Configuration.GetSection(SumitKumar.Application.Options.PortfolioOptions.SectionName));

        return builder.Build();
    }
}
