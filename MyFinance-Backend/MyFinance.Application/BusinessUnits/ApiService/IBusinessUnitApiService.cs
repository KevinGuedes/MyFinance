using FluentResults;
using MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.BusinessUnits.Commands.UpdateBusinessUnit;
using MyFinance.Application.BusinessUnits.ViewModels;

namespace MyFinance.Application.BusinessUnits.ApiService;

public interface IBusinessUnitApiService
{
    Task<Result<IEnumerable<BusinessUnitViewModel>>> GetBusinessUnitsAsync(CancellationToken cancellationToken);
    Task<Result<BusinessUnitViewModel>> CreateBusinessUnitAsync(CreateBusinessUnitCommand command, CancellationToken cancellationToken);
    Task<Result<BusinessUnitViewModel>> UpdateBusinessUnitAsync(UpdateBusinessUnitCommand command, CancellationToken cancellationToken);
}
