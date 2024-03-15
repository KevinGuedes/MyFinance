using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infrastructure.Data.Configurations;

public sealed class TransferConfiguration : EntityConfiguration<Transfer>
{
    public override void Configure(EntityTypeBuilder<Transfer> builder)
    {
        base.Configure(builder);

        builder.Property(transfer => transfer.SettlementDate).IsRequired();
        builder.Property(transfer => transfer.Description).IsRequired().HasMaxLength(300);
        builder.Property(transfer => transfer.RelatedTo).IsRequired().HasMaxLength(300);
        builder.Property(transfer => transfer.Type).HasConversion<string>().IsRequired();
        builder.Property(transfer => transfer.Value).IsRequired().HasPrecision(17, 2);
    }
}
