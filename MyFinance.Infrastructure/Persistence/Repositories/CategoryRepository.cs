using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

internal class CategoryRepository(MyFinanceDbContext myFinanceDbContext)
    : EntityRepository<Category>(myFinanceDbContext), ICategoryRepository
{
    public Task<long> GetTotalCountAsync(Guid managementUnitId, CancellationToken cancellationToken)
        => _myFinanceDbContext.AccountTags
            .Where(category => category.ManagementUnitId == managementUnitId)
            .LongCountAsync(cancellationToken);

    public async Task<IEnumerable<Category>> GetPaginatedAsync(
         Guid managementUnitId,
         int pageNumber,
         int pageSize,
         CancellationToken cancellationToken)
         => await _myFinanceDbContext.Categories
            .AsNoTracking()
            .Where(category => category.ManagementUnitId == managementUnitId)
            .OrderBy(category => category.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken)
        => _myFinanceDbContext.Categories
            .AnyAsync(category => category.Name == name, cancellationToken);

    public Task<Category?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken)
        => _myFinanceDbContext.Categories
            .AsNoTracking()
            .Where(category => category.Name == name)
            .FirstOrDefaultAsync(cancellationToken);
}
