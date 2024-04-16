using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infrastructure.Persistence.Configurations;

internal sealed class AccountTagConfiguration : EntityConfiguration<AccountTag>
{
    public override void Configure(EntityTypeBuilder<AccountTag> builder)
    {
        base.Configure(builder);

        builder.HasIndex(at => at.Tag).IsUnique();

        builder.Property(at => at.Tag).IsRequired().HasMaxLength(10);
        builder.Property(at => at.Description).IsRequired(false).HasMaxLength(300);
        builder.Property(bu => bu.ReasonToArchive).IsRequired(false).HasMaxLength(300);
        builder.Property(bu => bu.ArchivedOnUtc).IsRequired(false);
        builder.Property(bu => bu.IsArchived).IsRequired();

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(at => at.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}