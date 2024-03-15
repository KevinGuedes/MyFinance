using FluentResults;
using MyFinance.Application.UseCases.MonthlyBalances.DTOs;

namespace MyFinance.Application.Abstractions.ApiServices;

public interface IMonthlyBalanceApiService
{
    Task<Result<IEnumerable<MonthlyBalanceDTO>>> GetMonthlyBalancesAsync(
        Guid businessUnitId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}
