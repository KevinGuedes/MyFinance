using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infrastructure.Persistence.Configurations;

internal sealed class ManagementUnitConfiguration : EntityConfiguration<ManagementUnit>
{
    public override void Configure(EntityTypeBuilder<ManagementUnit> builder)
    {
        base.Configure(builder);

        builder.HasIndex(mu => new { mu.Name, mu.UserId }).IsUnique();

        builder.Property(mu => mu.Name).IsRequired().HasMaxLength(100);
        builder.Property(mu => mu.Income).IsRequired().HasColumnType("MONEY");
        builder.Property(mu => mu.Outcome).IsRequired().HasColumnType("MONEY");
        builder.Property(mu => mu.Description).IsRequired(false).HasMaxLength(300);
        builder.Property(mu => mu.ReasonToArchive).IsRequired(false).HasMaxLength(300);
        builder.Property(mu => mu.ArchivedOnUtc).IsRequired(false);
        builder.Property(mu => mu.IsArchived).IsRequired();
        builder.Ignore(mu => mu.Balance);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(mu => mu.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}