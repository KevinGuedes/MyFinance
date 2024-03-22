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
            .AnyAsync(bu => bu.Name == name, cancellationToken);

    public Task<BusinessUnit?> GetByNameAsync(string name, CancellationToken cancellationToken)
        => _myFinanceDbContext.BusinessUnits
            .Where(bu => bu.Name == name)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

    public Task<BusinessUnit?> GetWithSummaryData(Guid id, int year, CancellationToken cancellationToken)
        => _myFinanceDbContext.BusinessUnits
            .Include(bu => bu.MonthlyBalances
                .Where(mb => mb.Transfers.Count != 0 && mb.ReferenceYear == year)
                .OrderBy(mb => mb.ReferenceYear)
                .ThenBy(mb => mb.ReferenceMonth))
            .ThenInclude(mb => mb.Transfers
                .OrderByDescending(t => t.CreatedOnUtc)
                .ThenByDescending(t => t.RelatedTo))
            .ThenInclude(t => t.AccountTag)
            .AsNoTracking()
            .FirstOrDefaultAsync(bu => bu.Id == id, cancellationToken);
}