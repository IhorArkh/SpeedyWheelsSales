using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Infrastructure.Data.EntityConfigurations;

namespace SpeedyWheelsSales.Infrastructure.Data;

public class DataContext : IdentityDbContext
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
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new AppUserConfiguration());
        builder.ApplyConfiguration(new AdConfiguration());
        builder.ApplyConfiguration(new CarConfiguration());
        builder.ApplyConfiguration(new FavouriteAdConfiguration());
        builder.ApplyConfiguration(new PhotoConfiguration());
        builder.ApplyConfiguration(new SavedSearchConfiguration());
        builder.ApplyConfiguration(new MessageConfiguration());
    }
}