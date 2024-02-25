namespace SpeedyWheelsSales.WebAPI.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication();
        services.AddAuthorization();
        
        return services;
    }
}