using AutoMapper;
using FluentResults;
using MediatR;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Commands.UnarchiveBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;
using MyFinance.Application.UseCases.BusinessUnits.DTOs;
using MyFinance.Application.UseCases.BusinessUnits.Queries.GetBusinessUnits;

namespace MyFinance.Application.UseCases.BusinessUnits.ApiService;

public class BusinessUnitApiService : BaseApiService, IBusinessUnitApiService
{
    private readonly IMapper _mapper;
    public BusinessUnitApiService(IMediator mediator, IMapper mapper) : base(mediator)
        => _mapper = mapper;

    public async Task<Result<IEnumerable<BusinessUnitDTO>>> GetBusinessUnitsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = new GetBusinessUnitsQuery(page, pageSize);
        var result = await _mediator.Send(query, cancellationToken);
        return MapResult(result, _mapper.Map<IEnumerable<BusinessUnitDTO>>);
    }

    public async Task<Result<BusinessUnitDTO>> CreateBusinessUnitAsync(
        CreateBusinessUnitCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return MapResult(await _mediator.Send(command, cancellationToken), _mapper.Map<BusinessUnitDTO>);
    }

    public async Task<Result<BusinessUnitDTO>> UpdateBusinessUnitAsync(
        UpdateBusinessUnitCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return MapResult(result, _mapper.Map<BusinessUnitDTO>);
    }

    public Task<Result> ArchiveBusinessUnitAsync(ArchiveBusinessUnitCommand command, CancellationToken cancellationToken)
        => _mediator.Send(command, cancellationToken);

    public Task<Result> UnarchiveBusinessUnitAsync(Guid id, CancellationToken cancellationToken)
        => _mediator.Send(new UnarchiveBusinessUnitCommand(id), cancellationToken);
}
