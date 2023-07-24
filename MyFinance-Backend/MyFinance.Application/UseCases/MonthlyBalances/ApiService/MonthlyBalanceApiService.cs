using AutoMapper;
using FluentResults;
using MediatR;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.UseCases.MonthlyBalances.Queries.GetMonthlyBalances;
using MyFinance.Application.UseCases.MonthlyBalances.DTOs;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.MonthlyBalances.ApiService;

public class MonthlyBalanceApiService : EntityApiService, IMonthlyBalanceApiService
{
    public MonthlyBalanceApiService(IMediator mediator, IMapper mapper)
         : base(mediator, mapper)
    {
    }

    public async Task<Result<IReadOnlyCollection<MonthlyBalanceDTO>>> GetMonthlyBalancesAsync(
        Guid businessUnitId,
        int take,
        int skip,
        CancellationToken cancellationToken)
    {
        var query = new GetMonthlyBalancesQuery(businessUnitId, take, skip);
        var monthlyBalances = await _mediator.Send(query, cancellationToken);
        return MapResult<MonthlyBalance, MonthlyBalanceDTO>(monthlyBalances);
    }
}
