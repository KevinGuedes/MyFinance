using ClosedXML.Excel;
using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Domain.Interfaces;
using System.Globalization;

namespace MyFinance.Application.UseCases.Summary.Queries.GetMonthlyBalanceSummary;

internal sealed class GetMonthlyBalanceSummaryHandler : IQueryHandler<GetMonthlyBalanceSummaryQuery, Tuple<string, XLWorkbook>>
{
    private readonly ILogger<GetMonthlyBalanceSummaryHandler> _logger;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;
    public GetMonthlyBalanceSummaryHandler(
        ILogger<GetMonthlyBalanceSummaryHandler> logger,
        IMonthlyBalanceRepository monthlyBalanceRepository)
        => (_logger, _monthlyBalanceRepository) = (logger, monthlyBalanceRepository);

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

        var referenceDate = new DateOnly(monthlyBalance.ReferenceYear, monthlyBalance.ReferenceMonth, 1);
        var workSheetName = referenceDate.ToString("y", new CultureInfo("en-US"));
        var businessUnitName = monthlyBalance.BusinessUnit.Name;
        var fileName = string.Format("{0} - {1}.xlsx", businessUnitName, workSheetName);

        var workBook = new XLWorkbook();
        var workSheet = workBook.AddWorksheet(workSheetName);

        return Result.Ok(new Tuple<string, XLWorkbook>(fileName, workBook));
    }
}
