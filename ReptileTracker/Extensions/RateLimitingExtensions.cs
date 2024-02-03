using System.Threading.Tasks;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ReptileTracker.Extensions;

public static class RateLimitingServiceExtensions
{
    public static void RateLimitingLoggingExtensions(this IServiceCollection services)
    {
        services.AddHttpLogging(options =>
        {
            options.RequestHeaders.Add("Origin");
            options.RequestHeaders.Add("X-Client-Id");
            options.ResponseHeaders.Add("Retry-After");
            options.LoggingFields = HttpLoggingFields.All;
        });
    }

    public static async Task UseCustomClientRateLimiting(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var clientPolicyStore = scope.ServiceProvider
                .GetRequiredService<IClientPolicyStore>();
            await clientPolicyStore.SeedAsync();
        }

        app.UseClientRateLimiting();
    }

    public static IServiceCollection AddCustomRateLimiting(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        services.AddMemoryCache();
        services.AddInMemoryRateLimiting();

        services.Configure<ClientRateLimitOptions>(configurationManager.GetSection("ClientRateLimiting"));
        services.Configure<ClientRateLimitPolicies>(configurationManager.GetSection("ClientRateLimitPolicies"));
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        
        return services;
    }
}