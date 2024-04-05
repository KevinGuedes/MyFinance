using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

internal sealed class BusinessUnitRepository(MyFinanceDbContext myFinanceDbContext)
    : EntityRepository<BusinessUnit>(myFinanceDbContext), IBusinessUnitRepository
{
    public async Task<IEnumerable<BusinessUnit>> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
        => await _myFinanceDbContext.BusinessUnits
            .OrderByDescending(bu => bu.CreatedOnUtc)
            .ThenByDescending(bu => bu.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        => _myFinanceDbContext.BusinessUnits
            .AsNoTracking()
            .AnyAsync(bu => bu.Name == name, cancellationToken);

    public Task<BusinessUnit?> GetByNameAsync(string name, CancellationToken cancellationToken)
        => _myFinanceDbContext.BusinessUnits
            .AsNoTracking()
            .Where(bu => bu.Name == name)
            .FirstOrDefaultAsync(cancellationToken);
}