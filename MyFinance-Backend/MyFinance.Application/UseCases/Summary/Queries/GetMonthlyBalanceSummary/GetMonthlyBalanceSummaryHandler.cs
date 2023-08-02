using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2013.Word;
using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Application.Services.Spreadsheet;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;
using MyFinance.Domain.Interfaces;
using System;
using System.Globalization;

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

        return Result.Ok(_spreadsheetService.GetMonthlyBalanceSummary(monthlyBalance));
    }

}
