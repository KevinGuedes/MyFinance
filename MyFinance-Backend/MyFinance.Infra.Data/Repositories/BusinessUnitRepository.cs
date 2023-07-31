using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories;

public class BusinessUnitRepository : EntityRepository<BusinessUnit>, IBusinessUnitRepository
{
    public BusinessUnitRepository(MyFinanceDbContext myFinanceDbContext)
        : base(myFinanceDbContext) { }

    public async Task<IEnumerable<BusinessUnit>> GetAllPaginatedAsync(
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

    public Task<BusinessUnit?> GetWithMonthlyBalancesPaginated(
        Guid id,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
        => _myFinanceDbContext.BusinessUnits
            .Where(bu => bu.Id == id)
            .Include(bu => bu.MonthlyBalances
                .OrderByDescending(mb => mb.ReferenceYear)
                .ThenByDescending(mb => mb.ReferenceMonth)
                .Skip((page - 1) * pageSize)
                .Take(pageSize))
            .ThenInclude(mb => mb.Transfers)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
}
