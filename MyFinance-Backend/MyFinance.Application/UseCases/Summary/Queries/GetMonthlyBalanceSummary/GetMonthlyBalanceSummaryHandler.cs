using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Summary.Responses;

namespace MyFinance.Application.UseCases.Summary.Queries.GetMonthlyBalanceSummary;

internal sealed class GetMonthlyBalanceSummaryHandler(
    IMonthlyBalanceRepository monthlyBalanceRepository,
    ISpreadsheetService spreadsheetService)
    : IQueryHandler<GetMonthlyBalanceSummaryQuery, SummaryResponse>
{
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository = monthlyBalanceRepository;
    private readonly ISpreadsheetService _spreadsheetService = spreadsheetService;

    public async Task<Result<SummaryResponse>> Handle(GetMonthlyBalanceSummaryQuery query,
        CancellationToken cancellationToken)
    {
        var monthlyBalance = await _monthlyBalanceRepository.GetWithSummaryData(query.Id, cancellationToken);

        if (monthlyBalance is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Monthly Balance with Id {query.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        var hasTransfersForProcessing = monthlyBalance.Transfers.Count is not 0;
        if (!hasTransfersForProcessing)
        {
            var errorMessage = $"Monthly Balance with Id {query.Id} has no Transfers to summarize";
            var unprocessableEntityError = new UnprocessableEntityError(errorMessage);
            return Result.Fail(unprocessableEntityError);
        }

        var summaryData = _spreadsheetService.GetMonthlyBalanceSummary(monthlyBalance);

        return Result.Ok(SummaryMapper.DTR.Map(summaryData));
    }
}