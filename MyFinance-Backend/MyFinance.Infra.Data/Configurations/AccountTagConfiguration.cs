using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Data.Configurations;

public sealed class AccountTagConfiguration : EntityConfiguration<AccountTag>
{
    public override void Configure(EntityTypeBuilder<AccountTag> builder)
    {
        base.Configure(builder);

        builder.Property(at => at.Tag).IsRequired();
        builder.Property(at => at.Description);

        builder.HasMany(at => at.Transfers)
            .WithOne(t => t.AccountTag)
            .HasForeignKey(t => t.AccountTagId)
            .IsRequired();

        builder.HasMany(at => at.BusinessUnits)
            .WithMany(bu => bu.AccountTags);
    }
}
