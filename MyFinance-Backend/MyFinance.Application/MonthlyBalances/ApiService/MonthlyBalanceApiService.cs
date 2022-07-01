using AutoMapper;
using FluentResults;
using MediatR;
using MyFinance.Application.Generics.ApiService;
using MyFinance.Application.MonthlyBalances.Queries.GetRecentMonthlyBalances;
using MyFinance.Application.MonthlyBalances.ViewModels;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.MonthlyBalances.ApiService
{
    public class MonthlyBalanceApiService : EntityApiService, IMonthlyBalanceApiService
    {
        public MonthlyBalanceApiService(IMediator mediator, IMapper mapper)
             : base(mediator, mapper)
        {
        }

        public async Task<Result<IEnumerable<MonthlyBalanceViewModel>>> GetMonthlyBalancesAsync(
            GetMonthlyBalancesQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return ProcessResultAndMapIfSuccess<MonthlyBalance, MonthlyBalanceViewModel>(result);
        }
    }
}
