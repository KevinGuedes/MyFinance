using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

internal class CategoryRepository(MyFinanceDbContext myFinanceDbContext)
    : EntityRepository<Category>(myFinanceDbContext), ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetPaginatedAsync(
         int pageNumber,
         int pageSize,
         CancellationToken cancellationToken)
         => await _myFinanceDbContext.Categories
             .OrderBy(category => category.Name)
             .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize)
             .AsNoTracking()
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
            .Where(category => category.Name == name)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
}
