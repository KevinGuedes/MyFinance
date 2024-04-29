using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

namespace MyFinance.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : EntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.HasIndex(user => user.Email).IsUnique();

        builder.Property(user => user.Email).HasMaxLength(256).IsRequired();
        builder.Property(user => user.PasswordHash).IsRequired();
        builder.Property(user => user.Name).HasMaxLength(100).IsRequired();
        builder.Property(user => user.LastPasswordUpdateOnUtc).IsRequired(false);
        builder.Property(user => user.FailedSignInAttempts).IsRequired();
        builder.Property(user => user.LockoutEndOnUtc).IsRequired(false);
        builder.Property(user => user.IsEmailVerified).IsRequired();
    }
}