using MyFinance.Application.BusinessUnits.ViewModels;

namespace MyFinance.Application.BusinessUnits.ApiService
{
    public interface IBusinessUnitApiService
    {
        Task<BusinessUnitViewModel> CreateBusinessUnitAsync(BusinessUnitViewModel businessUnitViewModel, CancellationToken cancellationToken);
        Task<IEnumerable<BusinessUnitViewModel>> GetBusinessUnitsAsync(CancellationToken cancellationToken);
    }
}
