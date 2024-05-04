using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SpeedyWheelsSales.Infrastructure.Data.EntityConfigurations;

public class SavedSearchConfiguration : IEntityTypeConfiguration<SavedSearch>
{
    public void Configure(EntityTypeBuilder<SavedSearch> builder)
    {
        builder.Property(c => c.MinPrice).HasColumnType("decimal(18, 2)");
        builder.Property(c => c.MaxPrice).HasColumnType("decimal(18, 2)");

        builder.Property(x => x.QueryString).IsRequired();
        builder.Property(x => x.Brand).HasMaxLength(50);
        builder.Property(x => x.Model).HasMaxLength(50);
        builder.Property(x => x.EngineType).HasMaxLength(10);
        builder.Property(x => x.Transmission).HasMaxLength(10);
        builder.Property(x => x.TypeOfDrive).HasMaxLength(10);
        builder.Property(x => x.Color).HasMaxLength(20);
    }
}