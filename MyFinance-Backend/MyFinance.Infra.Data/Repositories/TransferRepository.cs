using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories;

public sealed class TransferRepository : EntityRepository<Transfer>, ITransferRepository
{
    public TransferRepository(MyFinanceDbContext myFinanceDbContext) : base(myFinanceDbContext)
    {
    }

    public override async Task<Transfer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _myFinanceDbContext.Transfers
            .Include(t => t.MonthlyBalance)
            .ThenInclude(mb => mb.BusinessUnit)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
}
