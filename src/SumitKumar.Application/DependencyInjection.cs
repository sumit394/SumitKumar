using Microsoft.Extensions.DependencyInjection;
using SumitKumar.Application.Abstractions;
using SumitKumar.Application.Services;

namespace SumitKumar.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IContactService, ContactService>();
        return services;
    }
}
