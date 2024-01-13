using Microsoft.Extensions.DependencyInjection;

namespace ReptileTracker.Extensions;

public static class CorsExtensions
{
    public static void CorsExtension(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: "ReptileTrackerDashboard",
                        policy =>
                        {
                            policy.WithOrigins("http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        });
        });
    }
 
}