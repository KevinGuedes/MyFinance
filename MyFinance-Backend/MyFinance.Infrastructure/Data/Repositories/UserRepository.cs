using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infrastructure.Data.Context;

namespace MyFinance.Infrastructure.Data.Repositories;
public class UserRepository(MyFinanceDbContext myFinanceDbContext)
    : EntityRepository<User>(myFinanceDbContext), IUserRepository
{
    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
        => _myFinanceDbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.Email == email, cancellationToken);

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => _myFinanceDbContext.Users.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
}
