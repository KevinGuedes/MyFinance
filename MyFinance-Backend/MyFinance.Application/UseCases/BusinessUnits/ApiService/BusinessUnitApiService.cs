using AutoMapper;
using FluentResults;
using MediatR;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Queries.GetBusinessUnits;
using MyFinance.Application.UseCases.BusinessUnits.DTOs;
using MyFinance.Domain.Entities;
using MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Commands.UnarchiveBusinessUnit;

namespace MyFinance.Application.UseCases.BusinessUnits.ApiService;

public class BusinessUnitApiService : EntityApiService, IBusinessUnitApiService
{
    public BusinessUnitApiService(IMediator mediator, IMapper mapper)
         : base(mediator, mapper)
    {
    }

    public async Task<Result<IEnumerable<BusinessUnitDTO>>> GetBusinessUnitsAsync(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBusinessUnitsQuery(), cancellationToken);
        return MapResult<BusinessUnit, BusinessUnitDTO>(result);
    }

    public async Task<Result<BusinessUnitDTO>> CreateBusinessUnitAsync(
        CreateBusinessUnitCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return MapResult<BusinessUnit, BusinessUnitDTO>(result);
    }

    public async Task<Result<BusinessUnitDTO>> UpdateBusinessUnitAsync(
        UpdateBusinessUnitCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return MapResult<BusinessUnit, BusinessUnitDTO>(result);
    }

    public Task<Result> ArchiveBusinessUnitAsync(ArchiveBusinessUnitCommand command, CancellationToken cancellationToken)
        => _mediator.Send(command, cancellationToken);

    public Task<Result> UnarchiveBusinessUnitAsync(Guid id, CancellationToken cancellationToken)
        => _mediator.Send(new UnarchiveBusinessUnitCommand(id), cancellationToken);
}
