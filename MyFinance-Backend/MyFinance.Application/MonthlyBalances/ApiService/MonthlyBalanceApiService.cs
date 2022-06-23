using AutoMapper;
using MediatR;
using MyFinance.Application.MonthlyBalances.Queries.GetRecentMonthlyBalances;
using MyFinance.Application.MonthlyBalances.ViewModels;

namespace MyFinance.Application.MonthlyBalances.ApiService
{
    public class MonthlyBalanceApiService : IMonthlyBalanceApiService
    {
        private protected readonly IMediator _mediator;
        private protected readonly IMapper _mapper;

        public MonthlyBalanceApiService(IMediator mediator, IMapper mapper)
             => (_mediator, _mapper) = (mediator, mapper);

        public async Task<IEnumerable<MonthlyBalanceViewModel>> GetMonthlyBalances(
            GetMonthlyBalancesQuery query, 
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return _mapper.Map<IEnumerable<MonthlyBalanceViewModel>>(result);
        }
    }
}
