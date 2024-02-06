﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Data.Configurations;
public sealed class UserConfiguration : EntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.HasIndex(user => user.Email).IsUnique();

        builder.Property(user => user.PasswordHash).IsRequired();
        builder.Property(transfer => transfer.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasMany(user => user.BusinessUnits)
            .WithOne(bu => bu.User)
            .HasForeignKey(bu => bu.UserId)
            .IsRequired();
    }
}