using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Data.Context;

namespace MyFinance.Infra.Data.Repositories;

public class TransferRepository : EntityRepository<Transfer>, ITransferRepository
{
    public TransferRepository(MyFinanceDbContext myFinanceDbContext)
      : base(myFinanceDbContext) { }
}
