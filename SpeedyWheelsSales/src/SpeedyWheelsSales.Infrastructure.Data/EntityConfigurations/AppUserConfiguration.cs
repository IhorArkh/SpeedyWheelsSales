﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SpeedyWheelsSales.Infrastructure.Data.EntityConfigurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(x => x.Email).HasMaxLength(50);
        builder.Property(x => x.UserName).IsRequired().HasMaxLength(40);
        builder.Property(x => x.PhoneNumber).HasMaxLength(20);
        builder.HasIndex(x => x.PhoneNumber).IsUnique();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Location).HasMaxLength(50);
        builder.Property(x => x.Bio).HasMaxLength(250);
    }
}