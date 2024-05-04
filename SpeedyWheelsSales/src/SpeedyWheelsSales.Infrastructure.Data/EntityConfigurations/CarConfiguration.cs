using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SpeedyWheelsSales.Infrastructure.Data.EntityConfigurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.Property(c => c.Price).HasColumnType("decimal(18, 2)");
        builder.Property(c => c.EngineSize).HasColumnType("decimal(18, 2)");

        builder.Property(x => x.Brand).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Model).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.Mileage).IsRequired();
        builder.Property(x => x.EngineSize).IsRequired();
        builder.Property(x => x.EngineSize).IsRequired();
        builder.Property(x => x.Vin).HasMaxLength(50);
        builder.Property(x => x.Plates).HasMaxLength(15);
        builder.Property(x => x.ManufactureDate).IsRequired();
        builder.Property(x => x.EngineType).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Transmission).IsRequired().HasMaxLength(10);
        builder.Property(x => x.TypeOfDrive).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Color).IsRequired().HasMaxLength(20);
    }
}