using MyFinance.Application.MonthlyBalances.Queries.GetRecentMonthlyBalances;
using MyFinance.Application.MonthlyBalances.ViewModels;

namespace MyFinance.Application.MonthlyBalances.ApiService
{
    public interface IMonthlyBalanceApiService
    {
        Task<IEnumerable<MonthlyBalanceViewModel>> GetMonthlyBalances(GetMonthlyBalancesQuery query, CancellationToken cancellationToken);
    }
}
