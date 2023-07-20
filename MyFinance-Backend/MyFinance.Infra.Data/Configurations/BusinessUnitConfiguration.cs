using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Data.Configurations;

public class BusinessUnitConfiguration : EntityConfiguration<BusinessUnit>
{
    public override void Configure(EntityTypeBuilder<BusinessUnit> builder)
    {
        base.Configure(builder);

        builder.HasIndex(bu => bu.Name).IsUnique();
        builder.Property(bu => bu.Name).IsRequired();

        builder.Property(bu => bu.IsArchived).IsRequired();
        builder.Property(bu => bu.CurrentBalance).IsRequired().HasPrecision(17, 4);
        builder.Property(bu => bu.Description).IsRequired(false);

        builder.HasMany(bu => bu.MonthlyBalances)
            .WithOne(mb => mb.BusinessUnit)
            .HasForeignKey(mb => mb.BusinessUnitId)
            .IsRequired();
    }
}
