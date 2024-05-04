using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Tests;

public static class InMemoryDbContextProvider
{
    public static async Task<DataContext> GetDbContext(string contextName)
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: contextName).Options;

        var databaseContext = new DataContext(options);
        await databaseContext.Database.EnsureDeletedAsync();
        await databaseContext.Database.EnsureCreatedAsync();

        return databaseContext;
    }
}