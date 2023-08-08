using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories;

public sealed class BusinessUnitRepository : EntityRepository<BusinessUnit>, IBusinessUnitRepository
{
    public BusinessUnitRepository(MyFinanceDbContext myFinanceDbContext) : base(myFinanceDbContext)
    {
    }

    public async Task<IEnumerable<BusinessUnit>> GetPaginatedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken)
        => await _myFinanceDbContext.BusinessUnits
            .OrderByDescending(bu => bu.CreationDate)
            .ThenByDescending(bu => bu.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        => _myFinanceDbContext.BusinessUnits
            .AsNoTracking()
            .AnyAsync(bu => bu.Name == name, cancellationToken);

    public Task<BusinessUnit?> GetByNameAsync(string name, CancellationToken cancellationToken)
        => _myFinanceDbContext.BusinessUnits
            .Where(bu => bu.Name == name)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

    public Task<BusinessUnit?> GetWithSummaryData(Guid id, CancellationToken cancellationToken)
        => _myFinanceDbContext.BusinessUnits
            .Include(bu => bu.MonthlyBalances
                .Where(mb => mb.Transfers.Any())
                .OrderBy(mb => mb.ReferenceYear)
                .ThenBy(mb => mb.ReferenceMonth))
            .ThenInclude(mb => mb.Transfers
               .OrderByDescending(t => t.CreationDate)
               .ThenByDescending(t => t.RelatedTo))
            .AsNoTracking()
            .FirstOrDefaultAsync(bu => bu.Id == id, cancellationToken);
}
