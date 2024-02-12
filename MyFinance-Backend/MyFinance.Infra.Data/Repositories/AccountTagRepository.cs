using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories;

public sealed class AccountTagRepository(MyFinanceDbContext myFinanceDbContext)
    : UserOwnedEntityRepository<AccountTag>(myFinanceDbContext), IAccountTagRepository
{
    public async Task<IEnumerable<AccountTag>> GetPaginatedAsync(
       int page,
       int pageSize,
       Guid userId, 
       CancellationToken cancellationToken)
        => await _myFinanceDbContext.AccountTags
            .Where(at => at.UserId == userId)
            .OrderByDescending(bu => bu.CreationDate)
            .ThenByDescending(bu => bu.Tag)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<bool> ExistsByTagAsync(
        string tag,
        Guid userId, 
        CancellationToken cancellationToken)
        => _myFinanceDbContext.AccountTags
            .Where(at => at.UserId == userId)
            .AnyAsync(at => at.Tag == tag, cancellationToken);

    public Task<AccountTag?> GetByTagAsync(
        string tag,
        Guid userId,
        CancellationToken cancellationToken)
        => _myFinanceDbContext.AccountTags
            .Where(at => at.Tag == tag && at.UserId == userId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
}
