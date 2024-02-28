using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<FavouriteAd>()
            .HasOne(x => x.AppUser)
            .WithMany(x => x.FavouriteAds)
            .HasForeignKey(x => x.AppUserId)
            .OnDelete(DeleteBehavior.NoAction); 
        
        builder.Entity<Car>()
            .Property(c => c.Price)
            .HasColumnType("decimal(18, 2)");

        builder.Entity<Car>()
            .Property(c => c.EngineSize)
            .HasColumnType("decimal(18, 2)");
    }
}