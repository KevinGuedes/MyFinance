using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;

namespace MyFinance.Infra.Data.Context;

public class MyFinanceDbContext : DbContext
{
    public DbSet<BusinessUnit> BusinessUnits { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<MonthlyBalance> MonthlyBalances { get; set; }

    public MyFinanceDbContext(DbContextOptions<MyFinanceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyFinanceDbContext).Assembly);
    }
}
