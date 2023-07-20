using FluentResults;
using MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.ViewModels;

namespace MyFinance.Application.UseCases.BusinessUnits.ApiService;

public interface IBusinessUnitApiService
{
    Task<Result<IEnumerable<BusinessUnitViewModel>>> GetBusinessUnitsAsync(CancellationToken cancellationToken);
    Task<Result<BusinessUnitViewModel>> CreateBusinessUnitAsync(CreateBusinessUnitCommand command, CancellationToken cancellationToken);
    Task<Result<BusinessUnitViewModel>> UpdateBusinessUnitAsync(UpdateBusinessUnitCommand command, CancellationToken cancellationToken);
}
