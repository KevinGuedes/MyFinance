using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Abstractions;
using MyFinance.Domain.Entities;
using MyFinance.Infrastructure.Persistence.Converters;
using System.Reflection;

namespace MyFinance.Infrastructure.Persistence.Context;

internal sealed class MyFinanceDbContext(
    DbContextOptions<MyFinanceDbContext> options,
    ICurrentUserProvider currentUserProvider) : DbContext(options)
{
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public DbSet<ManagementUnit> ManagementUnits { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<AccountTag> AccountTags { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        ApplyGlobalFiltersForUserOwnedEntities(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeValueConverter>();
        configurationBuilder.Properties<DateTime?>().HaveConversion<NullableDateTimeValueConverter>();
    }

    private void ApplyGlobalFiltersForUserOwnedEntities(ModelBuilder modelBuilder)
    {
        ApplyGlobalFilterForUserOwnedEntity<ManagementUnit>(modelBuilder);
        ApplyGlobalFilterForUserOwnedEntity<Transfer>(modelBuilder);
        ApplyGlobalFilterForUserOwnedEntity<AccountTag>(modelBuilder);
        ApplyGlobalFilterForUserOwnedEntity<Category>(modelBuilder);
    }

    private void ApplyGlobalFilterForUserOwnedEntity<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, IUserOwnedEntity
        => modelBuilder.Entity<TEntity>()
            .HasQueryFilter(entity => entity.UserId == _currentUserProvider.GetCurrentUserId());
}