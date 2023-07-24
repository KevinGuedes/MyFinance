using FluentResults;
using MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.DTOs;

namespace MyFinance.Application.UseCases.BusinessUnits.ApiService;

public interface IBusinessUnitApiService
{
    Task<Result<IReadOnlyCollection<BusinessUnitDTO>>> GetBusinessUnitsAsync(CancellationToken cancellationToken);
    Task<Result<BusinessUnitDTO>> CreateBusinessUnitAsync(CreateBusinessUnitCommand command, CancellationToken cancellationToken);
    Task<Result<BusinessUnitDTO>> UpdateBusinessUnitAsync(UpdateBusinessUnitCommand command, CancellationToken cancellationToken);
    Task<Result> ArchiveBusinessUnitAsync(ArchiveBusinessUnitCommand command, CancellationToken cancellationToken);
    Task<Result> UnarchiveBusinessUnitAsync(Guid id, CancellationToken cancellationToken);

}
