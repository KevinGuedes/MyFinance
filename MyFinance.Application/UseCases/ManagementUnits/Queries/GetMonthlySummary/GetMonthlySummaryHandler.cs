using FluentResults;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Summary.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetMonthlySummary;

internal sealed class GetMonthlySummaryHandler(
    IMyFinanceDbContext myFinanceDbContext,
    ISummaryService summaryService)
    : IQueryHandler<GetMonthlySummaryQuery, SummaryResponse>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;
    private readonly ISummaryService _summaryService = summaryService;

    public async Task<Result<SummaryResponse>> Handle(GetMonthlySummaryQuery query,
        CancellationToken cancellationToken)
    {
        var managementUnit = await _myFinanceDbContext.ManagementUnits
            .FindAsync([query.Id], cancellationToken);

        if (managementUnit is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Management Unit with Id {query.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        var transfers = await _myFinanceDbContext.Transfers
            .AsNoTracking()
            .Where(
                transfer => transfer.SettlementDate.Year == query.Year &&
                transfer.SettlementDate.Month == query.Month &&
                transfer.ManagementUnitId == query.Id)
            .Include(transfer => transfer.Category)
            .Include(transfer => transfer.AccountTag)
            .ToListAsync(cancellationToken);

        if (transfers.Count is 0)
        {
            var errorMessage = $"Management Unit with Id {query.Id} has no Transfers to summarize in the given period";
            var unprocessableEntityError = new UnprocessableEntityError(errorMessage);
            return Result.Fail(unprocessableEntityError);
        }

        var summaryData = _summaryService.GenerateMonthlySummary(managementUnit, transfers, query.Year, query.Month);

        return Result.Ok(SummaryMapper.DTR.Map(summaryData));
    }
}