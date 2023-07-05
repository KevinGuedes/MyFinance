using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Data.Configurations
{
    public class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(ar => ar.Id);
            builder.Property(ar => ar.Id).ValueGeneratedNever().IsRequired();
            builder.Property(ag => ag.CreationDate).IsRequired();
            builder.Property(ag => ag.UpdateDate).IsRequired(false);
        }
    }
}
