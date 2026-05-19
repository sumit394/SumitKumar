using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SumitKumar.Application;
using SumitKumar.Application.Abstractions;
using SumitKumar.Application.DTOs;
using SumitKumar.Infrastructure;
using SumitKumar.Infrastructure.Persistence;
using SumitKumar.Shared.Services;
using SumitKumar.Web.Client.Pages;
using SumitKumar.Web.Components;
using SumitKumar.Web.Components.Account;
using SumitKumar.Web.Data;

namespace SumitKumar.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents()
            .AddAuthenticationStateSerialization();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        builder.Services.AddAuthorization();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        // Clean-architecture composition
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

        // Typed API client used by Shared components during server-side prerender
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHttpClient<PortfolioApiClient>((sp, client) =>
        {
            var req = sp.GetService<IHttpContextAccessor>()?.HttpContext?.Request;
            client.BaseAddress = req is null
                ? new Uri("http://localhost/")
                : new Uri($"{req.Scheme}://{req.Host}");
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.EnsureCreated();
            scope.ServiceProvider.GetRequiredService<BlogDbContext>().Database.EnsureCreated();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();

        var api = app.MapGroup("/api").WithTags("Portfolio");

        api.MapGet("/blog", async (IBlogService svc, CancellationToken ct) =>
            Results.Ok(await svc.GetPublishedAsync(0, 100, ct)));

        api.MapGet("/blog/{slug}", async (string slug, IBlogService svc, CancellationToken ct) =>
        {
            var post = await svc.GetBySlugAsync(slug, ct);
            return post is null ? Results.NotFound() : Results.Ok(post);
        });

        api.MapPost("/contact", async (ContactMessageDto message, IContactService svc, CancellationToken ct) =>
        {
            await svc.SubmitAsync(message, ct);
            return Results.Accepted();
        }).DisableAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Client._Imports).Assembly, typeof(SumitKumar.Shared._Imports).Assembly);

        app.MapAdditionalIdentityEndpoints();

        app.Run();
    }
}
