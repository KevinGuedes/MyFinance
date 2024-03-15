using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;

namespace MyFinance.Infrastructure.Data.Context;

public sealed class MyFinanceDbContext(DbContextOptions<MyFinanceDbContext> options) : DbContext(options)
{
    public DbSet<BusinessUnit> BusinessUnits { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<MonthlyBalance> MonthlyBalances { get; set; }
    public DbSet<AccountTag> AccountTags { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyFinanceDbContext).Assembly);
    }
}
