using AutoMapper;
using MediatR;
using MyFinance.Application.Generics;
using MyFinance.Application.MonthlyBalances.Queries.GetRecentMonthlyBalances;
using MyFinance.Application.MonthlyBalances.ViewModels;

namespace MyFinance.Application.MonthlyBalances.ApiService
{
    public class MonthlyBalanceApiService : EntityApiService, IMonthlyBalanceApiService
    {
        public MonthlyBalanceApiService(IMediator mediator, IMapper mapper)
             : base(mediator, mapper)
        {
        }

        public async Task<IEnumerable<MonthlyBalanceViewModel>> GetMonthlyBalancesAsync(
            GetMonthlyBalancesQuery query, 
            CancellationToken cancellationToken)
            => _mapper.Map<IEnumerable<MonthlyBalanceViewModel>>(await _mediator.Send(query, cancellationToken));
    }
}
