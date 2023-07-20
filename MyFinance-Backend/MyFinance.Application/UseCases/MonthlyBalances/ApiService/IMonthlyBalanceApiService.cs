using FluentResults;
using MyFinance.Application.UseCases.MonthlyBalances.ViewModels;

namespace MyFinance.Application.UseCases.MonthlyBalances.ApiService;

public interface IMonthlyBalanceApiService
{
    Task<Result<IEnumerable<MonthlyBalanceViewModel>>> GetMonthlyBalancesAsync(
        Guid businessUnitId,
        int count,
        int skip,
        CancellationToken cancellationToken);
}
