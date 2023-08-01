namespace MyFinance.Application.UseCases.Summary.ApiService;

public interface ISummaryApiService
{
    Task<int> GenerateBusinessUnitSummaryAsync(Guid id, CancellationToken cancellationToken);
    Task<int> GenerateMonthlyBalanceSummaryAsync(Guid id, CancellationToken cancellationToken);
}
