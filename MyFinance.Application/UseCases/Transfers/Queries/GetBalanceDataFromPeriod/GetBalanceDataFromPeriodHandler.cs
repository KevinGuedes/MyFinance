using FluentResults;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetBalanceDataFromPeriod;

internal sealed class GetPeriodBalanceHandler(IMyFinanceDbContext myFinanceDbContext)
    : IQueryHandler<GetBalanceDataFromPeriodQuery, PeriodBalanceDataResponse>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<PeriodBalanceDataResponse>> Handle(GetBalanceDataFromPeriodQuery query, CancellationToken cancellationToken)
    {
        //check if everythign ok with db (entries and projections)
        var transfers = _myFinanceDbContext.Transfers
            .AsNoTracking()
            .Where(transfer => transfer.ManagementUnitId == query.ManagementUnitId);

        if (query.StartDate.HasValue && query.StartDate.Value != default)
            transfers = transfers.Where(transfer => transfer.SettlementDate >= query.StartDate.Value);

        if (query.EndDate.HasValue && query.EndDate.Value != default)
        {
            var endDateInEndOfTheDay = query.EndDate.Value.AddDays(1).AddTicks(-1);
            transfers = transfers.Where(transfer => transfer.SettlementDate <= endDateInEndOfTheDay);
        }

        if (query.CategoryId.HasValue && query.CategoryId.Value != default)
            transfers = transfers.Where(transfer => transfer.CategoryId == query.CategoryId.Value);

        if (query.AccountTagId.HasValue && query.AccountTagId.Value != default)
            transfers = transfers.Where(transfer => transfer.AccountTagId == query.AccountTagId.Value);

        var periodBalanceDataResponse = await transfers
            .GroupBy(transfer => true)
            .Select(transferGroup => new PeriodBalanceDataResponse
            {
                Income = transferGroup.Sum(transfer => transfer.Type == TransferType.Profit ? transfer.Value : 0),
                Outcome = transferGroup.Sum(transfer => transfer.Type == TransferType.Expense ? transfer.Value : 0)
            })
            .DefaultIfEmpty(new PeriodBalanceDataResponse
            {
                Income = 0m,
                Outcome = 0m,
            })
            .FirstAsync(cancellationToken);

        return Result.Ok(periodBalanceDataResponse);
    }
}
