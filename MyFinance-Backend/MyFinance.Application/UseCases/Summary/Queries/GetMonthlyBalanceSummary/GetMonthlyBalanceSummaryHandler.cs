using ClosedXML.Excel;
using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Application.Services.Spreadsheet;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.Summary.Queries.GetMonthlyBalanceSummary;

internal sealed class GetMonthlyBalanceSummaryHandler : IQueryHandler<GetMonthlyBalanceSummaryQuery, Tuple<string, XLWorkbook>>
{
    private readonly ILogger<GetMonthlyBalanceSummaryHandler> _logger;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;
    private readonly ISpreadsheetService _spreadsheetService;
    public GetMonthlyBalanceSummaryHandler(
        ILogger<GetMonthlyBalanceSummaryHandler> logger,
        IMonthlyBalanceRepository monthlyBalanceRepository,
        ISpreadsheetService spreadsheetService)
        => (_logger, _monthlyBalanceRepository, _spreadsheetService) = (logger, monthlyBalanceRepository, spreadsheetService);

    public async Task<Result<Tuple<string, XLWorkbook>>> Handle(GetMonthlyBalanceSummaryQuery query, CancellationToken cancellationToken)
    {
        var monthlyBalance = await _monthlyBalanceRepository.GetWithSummaryData(query.Id, cancellationToken);

        if (monthlyBalance is null)
        {
            _logger.LogWarning("Monthly Balance with Id {MonthlyBalanceId} not found", query.Id);
            var errorMessage = string.Format("Monthly Balance with Id {0} not found", query.Id);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var hasTransfersForProcessing = monthlyBalance.Transfers.Any();
        if (!hasTransfersForProcessing)
        {
            _logger.LogWarning("Monthly Balance with Id {MonthlyBalanceId} has no Transfers to summarize", query.Id);
            var errorMessage = string.Format("Monthly Balance with Id {0} has no Transfers to summarize", query.Id);
            var unprocessableEntityError = new UnprocessableEntityError(errorMessage);
            return Result.Fail(unprocessableEntityError);
        }

        return Result.Ok(_spreadsheetService.GetMonthlyBalanceSummary(monthlyBalance));
    }

}
