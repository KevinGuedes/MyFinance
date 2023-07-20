using AutoMapper;
using FluentResults;
using MediatR;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Queries.GetBusinessUnits;
using MyFinance.Application.UseCases.BusinessUnits.ViewModels;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.BusinessUnits.ApiService;

public class BusinessUnitApiService : EntityApiService, IBusinessUnitApiService
{
    public BusinessUnitApiService(IMediator mediator, IMapper mapper)
         : base(mediator, mapper)
    {
    }

    public async Task<Result<IEnumerable<BusinessUnitViewModel>>> GetBusinessUnitsAsync(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBusinessUnitsQuery(), cancellationToken);
        return MapResult<BusinessUnit, BusinessUnitViewModel>(result);
    }

    public async Task<Result<BusinessUnitViewModel>> CreateBusinessUnitAsync(
        CreateBusinessUnitCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return MapResult<BusinessUnit, BusinessUnitViewModel>(result);
    }

    public async Task<Result<BusinessUnitViewModel>> UpdateBusinessUnitAsync(
        UpdateBusinessUnitCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return MapResult<BusinessUnit, BusinessUnitViewModel>(result);
    }
}
