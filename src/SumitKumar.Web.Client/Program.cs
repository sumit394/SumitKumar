using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SumitKumar.Shared.Services;

namespace SumitKumar.Web.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddAuthenticationStateDeserialization();

        builder.Services.AddHttpClient<PortfolioApiClient>(c =>
            c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

        await builder.Build().RunAsync();
    }
}
