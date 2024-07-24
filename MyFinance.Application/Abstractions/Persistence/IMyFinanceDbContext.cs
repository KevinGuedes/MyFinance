using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence;

public interface IMyFinanceDbContext
{
    DbSet<ManagementUnit> ManagementUnits { get; set; }
    DbSet<Transfer> Transfers { get; set; }
    DbSet<AccountTag> AccountTags { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<User> Users { get; set; }
}
