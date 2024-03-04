using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SpeedyWheelsSales.Infrastructure.Data;

public static class DataServiceExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });

        return services;
    }
}