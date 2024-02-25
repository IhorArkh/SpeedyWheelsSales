using Domain;
using Microsoft.EntityFrameworkCore;

namespace SpeedyWheelsSales.Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    public DbSet<Ad> Ads { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<FavouriteAd> FavouriteAds { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<SavedSearch> SavedSearches { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
    }
}