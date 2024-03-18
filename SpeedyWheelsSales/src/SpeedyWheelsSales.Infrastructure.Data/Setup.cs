using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SpeedyWheelsSales.Infrastructure.Data;

public static class Setup
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });

        return services;
    }
}