using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Summary.Responses;

namespace MyFinance.Application.UseCases.BusinessUnits.Queries.GetMonthlySummary;

internal sealed class GetMonthlySummaryHandler(
    IBusinessUnitRepository businessUnitRepository,
    ISpreadsheetService spreadsheetService)
    : IQueryHandler<GetMonthlySummaryQuery, SummaryResponse>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ISpreadsheetService _spreadsheetService = spreadsheetService;

    public async Task<Result<SummaryResponse>> Handle(GetMonthlySummaryQuery query,
        CancellationToken cancellationToken)
    {
        var businessUnit = await _businessUnitRepository
            .GetWithSummaryData(query.Id, query.Year, query.Month, cancellationToken);

        if (businessUnit is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Business Unit with Id {query.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        var hasTransfersForProcessing = businessUnit.Transfers.Count is not 0;
        if (!hasTransfersForProcessing)
        {
            var errorMessage = $"Monthly Balance with Id {query.Id} has no Transfers to summarize";
            var unprocessableEntityError = new UnprocessableEntityError(errorMessage);
            return Result.Fail(unprocessableEntityError);
        }

        var summaryData = _spreadsheetService.GenerateMonthlySummary(businessUnit, query.Year, query.Month);

        return Result.Ok(SummaryMapper.DTR.Map(summaryData));
    }
}