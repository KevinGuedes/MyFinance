using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Domain.Entities;
using MyFinance.Infrastructure.Persistence.Context;

namespace MyFinance.Infrastructure.Persistence.Repositories;

internal sealed class TransferRepository(MyFinanceDbContext myFinanceDbContext)
    : EntityRepository<Transfer>(myFinanceDbContext), ITransferRepository
{
    public async Task<Transfer?> GetWithBusinessUnitByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _myFinanceDbContext.Transfers
            .Include(transfer => transfer.BusinessUnit)
            .FirstOrDefaultAsync(transfer => transfer.Id == id, cancellationToken);
}