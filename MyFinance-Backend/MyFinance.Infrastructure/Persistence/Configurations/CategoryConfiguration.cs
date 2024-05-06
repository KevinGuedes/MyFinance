using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infrastructure.Persistence.Configurations;

internal class CategoryConfiguration : EntityConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder
            .HasIndex(category => new { category.Name, category.UserId })
            .IsUnique();

        builder.Property(category => category.Name).IsRequired().HasMaxLength(50);
        builder.Property(category => category.ReasonToArchive).IsRequired(false).HasMaxLength(300);
        builder.Property(category => category.ArchivedOnUtc).IsRequired(false);
        builder.Property(category => category.IsArchived).IsRequired();

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(category => category.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
