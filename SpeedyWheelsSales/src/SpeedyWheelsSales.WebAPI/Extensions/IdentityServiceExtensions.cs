using Domain.Entities;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.WebAPI.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication();
        services.AddAuthorization();

        services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<DataContext>();
        
        return services;
    }
}