using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Common;

namespace MyFinance.Infrastructure.Persistence.Configurations;

internal abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.CreatedOnUtc).IsRequired();
        builder.Property(e => e.UpdatedOnUtc).IsRequired(false);
        builder.Property(e => e.RowVersion).IsRowVersion();
    }
}