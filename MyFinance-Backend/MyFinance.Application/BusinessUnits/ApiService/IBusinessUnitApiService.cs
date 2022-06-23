using MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.BusinessUnits.ViewModels;

namespace MyFinance.Application.BusinessUnits.ApiService
{
    public interface IBusinessUnitApiService
    {
        Task<BusinessUnitViewModel> CreateBusinessUnitAsync(CreateBusinessUnitCommand command, CancellationToken cancellationToken);
        Task<IEnumerable<BusinessUnitViewModel>> GetBusinessUnitsAsync(CancellationToken cancellationToken);
        Task DeleteBusinessUnitByIdAsync(Guid businessUnitId, CancellationToken cancellationToken);
    }
}
