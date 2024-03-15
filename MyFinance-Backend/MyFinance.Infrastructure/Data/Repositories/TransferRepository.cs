using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infrastructure.Data.Context;

namespace MyFinance.Infrastructure.Data.Repositories;

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
