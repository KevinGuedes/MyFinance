using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Summary.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetMonthlySummary;

internal sealed class GetMonthlySummaryHandler(
    IManagementUnitRepository managementUnitRepository,
    ITransferRepository transferRepository,
    ISummaryService summaryService)
    : IQueryHandler<GetMonthlySummaryQuery, SummaryResponse>
{
    private readonly IManagementUnitRepository _managementUnitRepository = managementUnitRepository;
    private readonly ITransferRepository _transferRepository = transferRepository;
    private readonly ISummaryService _summaryService = summaryService;

    public async Task<Result<SummaryResponse>> Handle(GetMonthlySummaryQuery query,
        CancellationToken cancellationToken)
    {
        var managementUnit = await _managementUnitRepository.GetByIdAsync(query.Id, cancellationToken);
        if (managementUnit is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Management Unit with Id {query.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        var transfers = await _transferRepository.GetWithSummaryDataAsync(
           query.Id,
           query.Year,
           query.Month,
           cancellationToken);

        var hasTransfersForProcessing = transfers.Count() is not 0;
        if (!hasTransfersForProcessing)
        {
            var errorMessage = $"Management Unit with Id {query.Id} has no Transfers to summarize in the given period";
            var unprocessableEntityError = new UnprocessableEntityError(errorMessage);
            return Result.Fail(unprocessableEntityError);
        }

        var summaryData = _summaryService.GenerateMonthlySummary(managementUnit, transfers, query.Year, query.Month);

        return Result.Ok(SummaryMapper.DTR.Map(summaryData));
    }
}