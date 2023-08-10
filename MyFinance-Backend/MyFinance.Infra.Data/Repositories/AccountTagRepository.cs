using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories;

public sealed class AccountTagRepository : EntityRepository<AccountTag>, IAccountTagRepository
{
    public AccountTagRepository(MyFinanceDbContext myFinanceDbContext) : base(myFinanceDbContext)
    {
    }

    public Task<bool> ExistsByTagAsync(string tag, CancellationToken cancellationToken)
        => _myFinanceDbContext.AccountTags.AnyAsync(at => at.Tag == tag, cancellationToken);

    public Task<AccountTag?> GetByTagAsync(string tag, CancellationToken cancellationToken)
        => _myFinanceDbContext.AccountTags
            .Where(at => at.Tag == tag)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
}
