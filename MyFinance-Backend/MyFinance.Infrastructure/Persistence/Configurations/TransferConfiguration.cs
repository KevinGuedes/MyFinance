using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infrastructure.Persistence.Configurations;

internal sealed class TransferConfiguration : EntityConfiguration<Transfer>
{
    public override void Configure(EntityTypeBuilder<Transfer> builder)
    {
        base.Configure(builder);

        builder.Property(transfer => transfer.SettlementDate).IsRequired();
        builder.Property(transfer => transfer.Description).IsRequired().HasMaxLength(300);
        builder.Property(transfer => transfer.RelatedTo).IsRequired().HasMaxLength(300);
        builder.Property(transfer => transfer.Type).HasConversion<string>().IsRequired();
        builder.Property(transfer => transfer.Value).IsRequired().HasColumnType("MONEY");

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(transfer => transfer.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(transfer => transfer.AccountTag)
            .WithMany(at => at.Transfers)
            .HasForeignKey(transfer => transfer.AccountTagId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(transfer => transfer.Category)
            .WithMany(category => category.Transfers)
            .HasForeignKey(transfer => transfer.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(transfer => transfer.ManagementUnit)
            .WithMany(bu => bu.Transfers)
            .HasForeignKey(transfer => transfer.ManagementUnitId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}