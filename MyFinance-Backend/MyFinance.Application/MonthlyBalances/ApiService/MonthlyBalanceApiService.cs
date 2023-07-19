using AutoMapper;
using FluentResults;
using MediatR;
using MyFinance.Application.Common.ApiService;
using MyFinance.Application.MonthlyBalances.Queries.GetMonthlyBalances;
using MyFinance.Application.MonthlyBalances.ViewModels;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.MonthlyBalances.ApiService;

public class MonthlyBalanceApiService : EntityApiService, IMonthlyBalanceApiService
{
    public MonthlyBalanceApiService(IMediator mediator, IMapper mapper)
         : base(mediator, mapper)
    {
    }

    public async Task<Result<IEnumerable<MonthlyBalanceViewModel>>> GetMonthlyBalancesAsync(
        Guid businessUnitId, 
        int take, 
        int skip, 
        CancellationToken cancellationToken)
    {
        var query = new GetMonthlyBalancesQuery(businessUnitId, take, skip);
        var result = await _mediator.Send(query, cancellationToken);
        return MapResult<MonthlyBalance, MonthlyBalanceViewModel>(result);
    }
}
