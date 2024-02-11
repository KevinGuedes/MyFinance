﻿using ClosedXML.Excel;
using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Infra.Services.Spreadsheet;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.Summary.Queries.GetMonthlyBalanceSummary;

internal sealed class GetMonthlyBalanceSummaryHandler(
    ILogger<GetMonthlyBalanceSummaryHandler> logger,
    IMonthlyBalanceRepository monthlyBalanceRepository,
    ISpreadsheetService spreadsheetService)
    : IQueryHandler<GetMonthlyBalanceSummaryQuery, Tuple<string, XLWorkbook>>
{
    private readonly ILogger<GetMonthlyBalanceSummaryHandler> _logger = logger;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository = monthlyBalanceRepository;
    private readonly ISpreadsheetService _spreadsheetService = spreadsheetService;

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

        var hasTransfersForProcessing = monthlyBalance.Transfers.Count is not 0;
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
