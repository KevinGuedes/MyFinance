using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

internal sealed class ManagementUnitRepository(MyFinanceDbContext myFinanceDbContext)
    : EntityRepository<ManagementUnit>(myFinanceDbContext), IManagementUnitRepository
{
    public async Task<IEnumerable<ManagementUnit>> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
        => await _myFinanceDbContext.ManagementUnits
            .AsNoTracking()
            .OrderBy(mu => mu.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        => _myFinanceDbContext.ManagementUnits
            .AsNoTracking()
            .AnyAsync(mu => mu.Name == name, cancellationToken);

    public Task<ManagementUnit?> GetByNameAsync(string name, CancellationToken cancellationToken)
        => _myFinanceDbContext.ManagementUnits
            .AsNoTracking()
            .Where(mu => mu.Name == name)
            .FirstOrDefaultAsync(cancellationToken);
}