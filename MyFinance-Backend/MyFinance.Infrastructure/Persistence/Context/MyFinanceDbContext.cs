﻿using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Abstractions;
using MyFinance.Domain.Entities;
using System.Reflection;

namespace MyFinance.Infrastructure.Persistence.Context;

public sealed class MyFinanceDbContext(
    DbContextOptions<MyFinanceDbContext> options,
    ICurrentUserProvider currentUserProvider) : DbContext(options)
{
    private readonly Guid? _currentUserId = currentUserProvider.GetCurrentUserId();

    public DbSet<BusinessUnit> BusinessUnits { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<MonthlyBalance> MonthlyBalances { get; set; }
    public DbSet<AccountTag> AccountTags { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        ApplyUserOwnedEntitiesGlobalFilters(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void ApplyUserOwnedEntitiesGlobalFilters(ModelBuilder modelBuilder)
    {
        if (_currentUserId.HasValue)
        {
            ApplyUserOwnedGlobalFilterFor<BusinessUnit>(modelBuilder);
            ApplyUserOwnedGlobalFilterFor<Transfer>(modelBuilder);
            ApplyUserOwnedGlobalFilterFor<MonthlyBalance>(modelBuilder);
            ApplyUserOwnedGlobalFilterFor<AccountTag>(modelBuilder);
        }
    }

    private void ApplyUserOwnedGlobalFilterFor<TEntity>(ModelBuilder modelBuilder)
        where TEntity : class, IUserOwnedEntity
        => modelBuilder.Entity<TEntity>().HasQueryFilter(entity => entity.UserId == _currentUserId!.Value);
}