using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SumitKumar.Application.Abstractions;
using SumitKumar.Application.Options;
using SumitKumar.Infrastructure.Email;
using SumitKumar.Infrastructure.Persistence;

namespace SumitKumar.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment env)
    {
        // Options binding with validation
        services.AddOptions<EmailOptions>()
            .Bind(configuration.GetSection(EmailOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<PortfolioOptions>()
            .Bind(configuration.GetSection(PortfolioOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Database
        var conn = configuration.GetConnectionString("BlogConnection")
                   ?? "Data Source=blog.db";
        services.AddDbContext<BlogDbContext>(opts => opts.UseSqlite(conn));

        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();

        // Email
        if (env.IsDevelopment())
        {
            services.AddSingleton<IEmailSender, NullEmailSender>();
        }
        else
        {
            services.AddSingleton<IEmailSender, SmtpEmailSender>();
        }

        return services;
    }
}
