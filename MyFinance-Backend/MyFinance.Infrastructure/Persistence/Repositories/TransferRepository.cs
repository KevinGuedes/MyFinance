using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

public sealed class TransferRepository(MyFinanceDbContext myFinanceDbContext)
    : UserOwnedEntityRepository<Transfer>(myFinanceDbContext), ITransferRepository
{
    public override async Task<Transfer?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        => await _myFinanceDbContext.Transfers
            .Where(t => t.UserId == userId)
            .Include(t => t.MonthlyBalance)
            .ThenInclude(mb => mb.BusinessUnit)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
}
