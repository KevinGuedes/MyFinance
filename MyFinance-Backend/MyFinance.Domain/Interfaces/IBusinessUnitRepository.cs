using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface IBusinessUnitRepository : IEntityRepository<BusinessUnit>
    {
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
    }
}
