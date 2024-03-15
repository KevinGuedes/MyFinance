using FluentResults;
using MediatR;
using MyFinance.Application.Abstractions.ApiServices;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.Mappers;
using MyFinance.Application.UseCases.MonthlyBalances.DTOs;
using MyFinance.Application.UseCases.MonthlyBalances.Queries.GetMonthlyBalances;

namespace MyFinance.Application.UseCases.MonthlyBalances.ApiService;

public sealed class MonthlyBalanceApiService(IMediator mediator) : BaseApiService(mediator), IMonthlyBalanceApiService
{
    public async Task<Result<IEnumerable<MonthlyBalanceDTO>>> GetMonthlyBalancesAsync(
        Guid businessUnitId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = new GetMonthlyBalancesQuery(businessUnitId, page, pageSize);
        var result = await _mediator.Send(query, cancellationToken);
        return MapResult(result, DomainToDTOMapper.MonthlyBalanceToDTO);
    }
}