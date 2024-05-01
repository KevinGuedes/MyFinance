using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

internal sealed class AccountTagRepository(MyFinanceDbContext myFinanceDbContext)
    : EntityRepository<AccountTag>(myFinanceDbContext), IAccountTagRepository
{
    public async Task<IEnumerable<AccountTag>> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
        => await _myFinanceDbContext.AccountTags
            .OrderBy(at => at.Tag)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<bool> ExistsByTagAsync(
        string tag,
        CancellationToken cancellationToken)
        => _myFinanceDbContext.AccountTags
            .AnyAsync(at => at.Tag == tag, cancellationToken);

    public Task<AccountTag?> GetByTagAsync(
        string tag,
        CancellationToken cancellationToken)
        => _myFinanceDbContext.AccountTags
            .Where(at => at.Tag == tag)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
}