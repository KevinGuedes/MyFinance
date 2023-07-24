using FluentResults;
using MyFinance.Application.UseCases.MonthlyBalances.DTOs;

namespace MyFinance.Application.UseCases.MonthlyBalances.ApiService;

public interface IMonthlyBalanceApiService
{
    Task<Result<IReadOnlyCollection<MonthlyBalanceDTO>>> GetMonthlyBalancesAsync(
        Guid businessUnitId,
        int count,
        int skip,
        CancellationToken cancellationToken);
}
