using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Data.Configurations;

public sealed class BusinessUnitConfiguration : EntityConfiguration<BusinessUnit>
{
    public override void Configure(EntityTypeBuilder<BusinessUnit> builder)
    {
        base.Configure(builder);

        builder.HasIndex(bu => bu.Name).IsUnique();
        builder.Property(bu => bu.Name).IsRequired().HasMaxLength(100);

        builder.Property(bu => bu.Income).IsRequired().HasPrecision(17, 2);
        builder.Property(bu => bu.Outcome).IsRequired().HasPrecision(17, 2);
        builder.Property(bu => bu.Description).IsRequired(false).HasMaxLength(300);
        builder.Property(bu => bu.ReasonToArchive).IsRequired(false).HasMaxLength(300);
        builder.Property(bu => bu.ArchiveDate).IsRequired(false);
        builder.Property(bu => bu.IsArchived).IsRequired();
        builder.Ignore(bu => bu.Balance);
    }
}
