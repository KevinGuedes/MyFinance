using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Data.Configurations;

public class MonthlyBalanceConfiguration : EntityConfiguration<MonthlyBalance>
{
    public override void Configure(EntityTypeBuilder<MonthlyBalance> builder)
    {
        base.Configure(builder);

        builder.Property(mb => mb.Income).IsRequired().HasPrecision(17, 4);
        builder.Property(mb => mb.Outcome).IsRequired().HasPrecision(17, 4);
        builder.Property(mb => mb.ReferenceYear).IsRequired();
        builder.Property(mb => mb.ReferenceMonth).IsRequired();

        builder.HasMany(mb => mb.Transfers)
            .WithOne(t => t.MonthlyBalance)
            .HasForeignKey(t => t.MonthlyBalanceId)
            .IsRequired();
    }
}
