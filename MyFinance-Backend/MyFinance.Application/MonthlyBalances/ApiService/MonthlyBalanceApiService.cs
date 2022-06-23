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

        public async Task<IEnumerable<MonthlyBalanceViewModel>> GetMonthlyBalances(
            GetMonthlyBalancesQuery query, 
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return _mapper.Map<IEnumerable<MonthlyBalanceViewModel>>(result);
        }
    }
}
