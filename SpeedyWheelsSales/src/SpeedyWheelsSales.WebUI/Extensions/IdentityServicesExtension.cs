using System.Net;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.WebUI.Extensions;

public static class IdentityServicesExtension
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication()
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                o.Cookie.Name = "AuthCookie";
                o.Events.OnRedirectToLogin = UnAuthorizedResponse;
                o.Events.OnRedirectToAccessDenied = UnAuthorizedResponse;
            });
        services.AddAuthorization();

        services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DataContext>();
        
        return services;
    }
    
    private static Task UnAuthorizedResponse(RedirectContext<CookieAuthenticationOptions> context)
    {
        context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
        return Task.CompletedTask;
    }
}