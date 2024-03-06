using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SpeedyWheelsSales.Infrastructure.Data.EntityConfigurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.Property(c => c.Price).HasColumnType("decimal(18, 2)");
        builder.Property(c => c.EngineSize).HasColumnType("decimal(18, 2)");

        builder.Property(x => x.Model).IsRequired();
        builder.Property(x => x.Model).HasMaxLength(50);
        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.Mileage).IsRequired();
        builder.Property(x => x.EngineSize).IsRequired();
        builder.Property(x => x.EngineSize).IsRequired();
        builder.Property(x => x.ManufactureDate).IsRequired();
        builder.Property(x => x.EngineType).IsRequired();
        builder.Property(x => x.Transmission).IsRequired();
        builder.Property(x => x.TypeOfDrive).IsRequired();
    }
}