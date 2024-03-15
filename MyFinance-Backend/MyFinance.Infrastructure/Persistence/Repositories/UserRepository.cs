using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

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