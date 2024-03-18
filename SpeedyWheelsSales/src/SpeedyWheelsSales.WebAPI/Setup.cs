using Domain.Entities;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.WebAPI;

public static class Setup
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication();
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