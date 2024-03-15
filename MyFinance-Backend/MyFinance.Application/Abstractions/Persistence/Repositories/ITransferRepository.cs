using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface ITransferRepository : IUserOwnedEntityRepository<Transfer>
{
}