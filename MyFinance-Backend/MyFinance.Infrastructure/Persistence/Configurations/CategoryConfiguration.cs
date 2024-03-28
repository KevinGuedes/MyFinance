using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;

namespace MyFinance.Infrastructure.Persistence.Configurations;

internal class CategoryConfiguration : EntityConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.HasIndex(at => at.Name).IsUnique();
        builder.Property(at => at.Name).IsRequired().HasMaxLength(50);

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
