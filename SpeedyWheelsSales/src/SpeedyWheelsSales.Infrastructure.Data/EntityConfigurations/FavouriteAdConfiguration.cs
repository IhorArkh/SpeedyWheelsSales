using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SpeedyWheelsSales.Infrastructure.Data.EntityConfigurations;

public class FavouriteAdConfiguration : IEntityTypeConfiguration<FavouriteAd>
{
    public void Configure(EntityTypeBuilder<FavouriteAd> builder)
    {
        builder.HasOne(x => x.AppUser)
            .WithMany(x => x.FavouriteAds)
            .HasForeignKey(x => x.AppUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}