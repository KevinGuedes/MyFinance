using ClosedXML.Excel;
using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Application.Services.Spreadsheet;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;

internal sealed class GetBusinessUnitSummaryHandler : IQueryHandler<GetBusinessUnitSummaryQuery, Tuple<string, XLWorkbook>>
{
    private readonly ILogger<GetBusinessUnitSummaryHandler> _logger;
    private readonly IBusinessUnitRepository _businessUnitRepository;
    private readonly ISpreadsheetService _spreadsheetService;

    public GetBusinessUnitSummaryHandler(
        ILogger<GetBusinessUnitSummaryHandler> logger,
        IBusinessUnitRepository businessUnitRepository,
        ISpreadsheetService spreadsheetService)
        => (_logger, _businessUnitRepository, _spreadsheetService) = (logger, businessUnitRepository, spreadsheetService);

    public async Task<Result<Tuple<string, XLWorkbook>>> Handle(GetBusinessUnitSummaryQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving Business Unit with Id {BusinessUnitId} ", query.Id);
        var businessUnit = await _businessUnitRepository.GetWithSummaryData(query.Id, cancellationToken);

        if (businessUnit is null)
        {
            _logger.LogWarning("Business Unit with Id {BusinessUnitId} not found", query.Id);
            var errorMessage = string.Format("Business Unit with Id {0} not found", query.Id);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var monthlyBalancesForProcessing = businessUnit.MonthlyBalances.Where(mb => mb.Transfers.Count > 0);
        if (!monthlyBalancesForProcessing.Any())
        {
            _logger.LogWarning("Business Unit with Id {BusinessUnitId} has no Monthly Balances with Transfers to summarize", query.Id);
            var errorMessage = string.Format("Business Unit with Id {0} has no Monthly Balances with Transfers to summarize", query.Id);
            var unprocessableEntityError = new UnprocessableEntityError(errorMessage);
            return Result.Fail(unprocessableEntityError);
        }

        return Result.Ok(_spreadsheetService.GetBusinessUnitSummary(businessUnit));
    }
}
