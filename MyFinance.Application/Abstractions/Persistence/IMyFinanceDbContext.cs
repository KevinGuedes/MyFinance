using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence;

public interface IMyFinanceDbContext
{
    DbSet<ManagementUnit> ManagementUnits { get; }
    DbSet<Transfer> Transfers { get; }
    DbSet<AccountTag> AccountTags { get; }
    DbSet<Category> Categories { get; }
    DbSet<User> Users { get; }
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    ChangeTracker ChangeTracker { get; }
}
