using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infrastructure.Persistence.Configurations;

public sealed class MonthlyBalanceConfiguration : EntityConfiguration<MonthlyBalance>
{
    public override void Configure(EntityTypeBuilder<MonthlyBalance> builder)
    {
        base.Configure(builder);

        builder.Property(mb => mb.Income).IsRequired().HasPrecision(17, 2);
        builder.Property(mb => mb.Outcome).IsRequired().HasPrecision(17, 2);
        builder.Property(mb => mb.ReferenceYear).IsRequired();
        builder.Property(mb => mb.ReferenceMonth).IsRequired();
        builder.Ignore(mb => mb.Balance);
    }
}
