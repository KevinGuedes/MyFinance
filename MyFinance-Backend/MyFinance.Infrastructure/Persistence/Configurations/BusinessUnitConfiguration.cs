using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infrastructure.Persistence.Configurations;

internal sealed class BusinessUnitConfiguration : EntityConfiguration<BusinessUnit>
{
    public override void Configure(EntityTypeBuilder<BusinessUnit> builder)
    {
        base.Configure(builder);

        builder.HasIndex(bu => bu.Name).IsUnique();

        builder.Property(bu => bu.Name).IsRequired().HasMaxLength(100);
        builder.Property(bu => bu.Income).IsRequired().HasColumnType("MONEY");
        builder.Property(bu => bu.Outcome).IsRequired().HasColumnType("MONEY");
        builder.Property(bu => bu.Description).IsRequired(false).HasMaxLength(300);
        builder.Property(bu => bu.ReasonToArchive).IsRequired(false).HasMaxLength(300);
        builder.Property(bu => bu.ArchivedOnUtc).IsRequired(false);
        builder.Property(bu => bu.IsArchived).IsRequired();
        builder.Ignore(bu => bu.Balance);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(bu => bu.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}