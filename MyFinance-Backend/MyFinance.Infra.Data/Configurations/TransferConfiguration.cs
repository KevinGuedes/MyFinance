using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Data.Configurations;

public class TransferConfiguration : EntityConfiguration<Transfer>
{
    public override void Configure(EntityTypeBuilder<Transfer> builder)
    {
        base.Configure(builder);

        builder.Property(transfer => transfer.SettlementDate).IsRequired();
        builder.Property(transfer => transfer.Description).IsRequired();
        builder.Property(transfer => transfer.RelatedTo).IsRequired();
        builder.Property(transfer => transfer.Type).HasConversion<string>().IsRequired();
        builder.Property(transfer => transfer.Value).IsRequired();
    }
}
