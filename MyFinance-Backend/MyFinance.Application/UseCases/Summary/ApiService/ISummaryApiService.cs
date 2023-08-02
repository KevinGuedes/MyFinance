using ClosedXML.Excel;
using FluentResults;

namespace MyFinance.Application.UseCases.Summary.ApiService;

public interface ISummaryApiService
{
    Task<Result<Tuple<string, XLWorkbook>>> GetBusinessUnitSummaryAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<Tuple<string, byte[]>>> GetMonthlyBalanceSummaryAsync(Guid id, CancellationToken cancellationToken);
}
