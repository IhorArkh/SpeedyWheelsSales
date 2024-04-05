using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.WebAPI;

public static class Setup
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.Authority = "https://localhost:5005";
                opt.Audience = "speedywheelssalesapi";

                opt.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                // TODO think about better token validation
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
}