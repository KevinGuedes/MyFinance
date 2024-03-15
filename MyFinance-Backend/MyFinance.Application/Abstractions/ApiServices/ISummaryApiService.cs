using FluentResults;

namespace MyFinance.Application.Abstractions.ApiServices;

public interface ISummaryApiService
{
    Task<Result<Tuple<string, byte[]>>> GetBusinessUnitSummaryAsync(Guid id, int year, CancellationToken cancellationToken);
    Task<Result<Tuple<string, byte[]>>> GetMonthlyBalanceSummaryAsync(Guid id, CancellationToken cancellationToken);
}
