using ClosedXML.Excel;
using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Services.Spreadsheet;

namespace MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;

internal sealed class GetBusinessUnitSummaryHandler(
    ILogger<GetBusinessUnitSummaryHandler> logger,
    IBusinessUnitRepository businessUnitRepository,
    ISpreadsheetService spreadsheetService)
    : IQueryHandler<GetBusinessUnitSummaryQuery, Tuple<string, XLWorkbook>>
{
    private readonly ILogger<GetBusinessUnitSummaryHandler> _logger = logger;
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ISpreadsheetService _spreadsheetService = spreadsheetService;

    public async Task<Result<Tuple<string, XLWorkbook>>> Handle(GetBusinessUnitSummaryQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving Business Unit with Id {BusinessUnitId} ", query.Id);
        var businessUnit = await _businessUnitRepository.GetWithSummaryData(query.Id, query.Year, cancellationToken);

        if (businessUnit is null)
        {
            _logger.LogWarning("Business Unit with Id {BusinessUnitId} not found", query.Id);
            var errorMessage = string.Format("Business Unit with Id {0} not found", query.Id);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var hasMonthlyBalancesForProcessing = businessUnit.MonthlyBalances.Count is not 0;
        if (!hasMonthlyBalancesForProcessing)
        {
            _logger.LogWarning("Business Unit with Id {BusinessUnitId} has no Monthly Balances with Transfers to summarize", query.Id);
            var errorMessage = string.Format("Business Unit with Id {0} has no Monthly Balances with Transfers to summarize", query.Id);
            var unprocessableEntityError = new UnprocessableEntityError(errorMessage);
            return Result.Fail(unprocessableEntityError);
        }

        return Result.Ok(_spreadsheetService.GetBusinessUnitSummary(businessUnit, query.Year));
    }
}
