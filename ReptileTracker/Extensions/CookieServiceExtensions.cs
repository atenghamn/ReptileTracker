using System;
using Microsoft.Extensions.DependencyInjection;

namespace ReptileTracker.Extensions;

public static class CookieServiceExtensions
{
    public static void CookieExtensions(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            options.LoginPath = "/account/login";
            options.AccessDeniedPath = "/account/accessdenied";
            options.SlidingExpiration = true;
        });
    }
}