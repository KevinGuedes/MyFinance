using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories;

public sealed class AccountTagRepository(MyFinanceDbContext myFinanceDbContext) 
    : EntityRepository<AccountTag>(myFinanceDbContext), IAccountTagRepository
{
    public async Task<IEnumerable<AccountTag>> GetPaginatedAsync(
       int page,
       int pageSize,
       CancellationToken cancellationToken)
       => await _myFinanceDbContext.AccountTags
            .OrderByDescending(bu => bu.CreationDate)
            .ThenByDescending(bu => bu.Tag)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<bool> ExistsByTagAsync(string tag, CancellationToken cancellationToken)
        => _myFinanceDbContext.AccountTags.AnyAsync(at => at.Tag == tag, cancellationToken);

    public Task<AccountTag?> GetByTagAsync(string tag, CancellationToken cancellationToken)
        => _myFinanceDbContext.AccountTags
            .Where(at => at.Tag == tag)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
}
