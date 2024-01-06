using Microsoft.Extensions.DependencyInjection;

namespace ReptileTracker.Extensions;

public static class CorsExtensions
{
    public static void CorsExtension(this IServiceCollection services)
    {
        // services.AddCors(options =>
        // {
        //     options.AddPolicy(name: "SomeExamplePolicy",
        //         policy: policy => policy.WithOrigin("https://localhost:1337"));
        // });
        
    }
 
}