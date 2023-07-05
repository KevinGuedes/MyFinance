using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Data.Configurations
{
    public class MonthlyBalanceConfiguration : EntityConfiguration<MonthlyBalance>
    {
        public override void Configure(EntityTypeBuilder<MonthlyBalance> builder)
        {
            base.Configure(builder);

            builder.OwnsOne(mb => mb.ReferenceData);

            builder.Property(mb => mb.CurrentBalance).IsRequired().HasPrecision(17, 2);
            
            builder.HasMany(mb => mb.Transfers)
                .WithOne(t => t.MonthlyBalance)
                .HasForeignKey(t => t.MonthlyBalanceId)
                .IsRequired();
        }
    }
}
