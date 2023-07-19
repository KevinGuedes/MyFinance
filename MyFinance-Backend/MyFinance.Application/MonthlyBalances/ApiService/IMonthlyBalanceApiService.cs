using FluentResults;
using MyFinance.Application.MonthlyBalances.ViewModels;

namespace MyFinance.Application.MonthlyBalances.ApiService;

public interface IMonthlyBalanceApiService
{
    Task<Result<IEnumerable<MonthlyBalanceViewModel>>> GetMonthlyBalancesAsync(
        Guid businessUnitId,  
        int count, 
        int skip,
        CancellationToken cancellationToken);
}
