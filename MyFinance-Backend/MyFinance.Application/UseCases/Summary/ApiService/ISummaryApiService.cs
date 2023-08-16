using FluentResults;

namespace MyFinance.Application.UseCases.Summary.ApiService;

public interface ISummaryApiService
{
    Task<Result<Tuple<string, byte[]>>> GetBusinessUnitSummaryAsync(Guid id, int year, CancellationToken cancellationToken);
    Task<Result<Tuple<string, byte[]>>> GetMonthlyBalanceSummaryAsync(Guid id, CancellationToken cancellationToken);
}
