using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

public sealed class BusinessUnitRepository(MyFinanceDbContext myFinanceDbContext)
    : UserOwnedEntityRepository<BusinessUnit>(myFinanceDbContext), IBusinessUnitRepository
{
    public async Task<IEnumerable<BusinessUnit>> GetPaginatedAsync(
        int page,
        int pageSize,
        Guid userId,
        CancellationToken cancellationToken)
        => await _myFinanceDbContext.BusinessUnits
            .Where(bu => bu.UserId == userId)
            .OrderByDescending(bu => bu.CreationDate)
            .ThenByDescending(bu => bu.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<bool> ExistsByNameAsync(string name, Guid userId, CancellationToken cancellationToken)
        => _myFinanceDbContext.BusinessUnits
            .Where(bu => bu.UserId == userId)
            .AnyAsync(bu => bu.Name == name, cancellationToken);

    public Task<BusinessUnit?> GetByNameAsync(string name, Guid userId, CancellationToken cancellationToken)
        => _myFinanceDbContext.BusinessUnits
            .Where(bu => bu.Name == name && bu.UserId == userId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

    public Task<BusinessUnit?> GetWithSummaryData(Guid id, int year, Guid userId, CancellationToken cancellationToken)
        => _myFinanceDbContext.BusinessUnits
            .Where(bu => bu.UserId == userId)
            .Include(bu => bu.MonthlyBalances
                .Where(mb => mb.Transfers.Count != 0 && mb.ReferenceYear == year)
                .OrderBy(mb => mb.ReferenceYear)
                .ThenBy(mb => mb.ReferenceMonth))
            .ThenInclude(mb => mb.Transfers
               .OrderByDescending(t => t.CreationDate)
               .ThenByDescending(t => t.RelatedTo))
            .ThenInclude(t => t.AccountTag)
            .AsNoTracking()
            .FirstOrDefaultAsync(bu => bu.Id == id, cancellationToken);
}
