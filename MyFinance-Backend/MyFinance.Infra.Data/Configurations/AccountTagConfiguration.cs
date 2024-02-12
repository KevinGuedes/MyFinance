using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Data.Configurations;

public sealed class AccountTagConfiguration : EntityConfiguration<AccountTag>
{
    public override void Configure(EntityTypeBuilder<AccountTag> builder)
    {
        base.Configure(builder);

        builder.HasIndex(at => at.Tag).IsUnique();
        builder.Property(at => at.Tag).IsRequired();

        builder.Property(at => at.Description).IsRequired(false);
        builder.Property(bu => bu.IsArchived).IsRequired();
        builder.Property(bu => bu.ReasonToArchive).IsRequired(false);
        builder.Property(bu => bu.ArchiveDate).IsRequired(false);
    }
}
