using MyFinance.Application.MonthlyBalances.Queries.GetRecentMonthlyBalances;
using MyFinance.Application.MonthlyBalances.ViewModels;

namespace MyFinance.Application.MonthlyBalances.ApiService
{
    public interface IMonthlyBalanceApiService
    {
        Task<IEnumerable<MonthlyBalanceViewModel>> GetMonthlyBalancesAsync(GetMonthlyBalancesQuery query, CancellationToken cancellationToken);
    }
}
