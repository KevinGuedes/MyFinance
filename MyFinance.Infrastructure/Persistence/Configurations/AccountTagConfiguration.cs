using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infrastructure.Persistence.Configurations;

internal sealed class AccountTagConfiguration : EntityConfiguration<AccountTag>
{
    public override void Configure(EntityTypeBuilder<AccountTag> builder)
    {
        base.Configure(builder);

        builder.HasIndex(at => new { at.Tag, at.UserId }).IsUnique();

        builder.Property(at => at.Tag).IsRequired().HasMaxLength(10);
        builder.Property(at => at.Description).IsRequired(false).HasMaxLength(300);
        builder.Property(at => at.ReasonToArchive).IsRequired(false).HasMaxLength(300);
        builder.Property(at => at.ArchivedOnUtc).IsRequired(false);
        builder.Property(at => at.IsArchived).IsRequired();

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(at => at.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
           .HasOne(at => at.ManagementUnit)
           .WithMany(mu => mu.AccountTags)
           .HasForeignKey(at => at.ManagementUnitId)
           .OnDelete(DeleteBehavior.NoAction);
    }
}