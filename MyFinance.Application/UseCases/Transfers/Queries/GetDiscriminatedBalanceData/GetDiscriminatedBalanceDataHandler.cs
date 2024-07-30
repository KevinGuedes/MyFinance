using FluentResults;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Helpers;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Enums;
using System.Globalization;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetDiscriminatedBalanceData;

internal sealed class GetDiscriminatedBalanceDataHandler(IMyFinanceDbContext myFinanceDbContext)
    : IQueryHandler<GetDiscriminatedBalanceDataQuery, DiscriminatedBalanceDataResponse>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<DiscriminatedBalanceDataResponse>> Handle(
        GetDiscriminatedBalanceDataQuery query,
        CancellationToken cancellationToken)
    {
        var (startDate, endDate) = MonthHelper.GetDateRangeInUTCFromPastMonths(
            query.PastMonths,
            query.IncludeCurrentMonth);

        var monthsInRange = MonthHelper.GetMonthsInRange(startDate, endDate);

        var groupedTransfers = await _myFinanceDbContext.Transfers
            .AsNoTracking()
            .Where(transfer =>
                 transfer.ManagementUnitId == query.ManagementUnitId &&
                 transfer.SettlementDate >= startDate &&
                 transfer.SettlementDate <= endDate)
            .GroupBy(transfer => new
            {
                transfer.SettlementDate.Month,
                transfer.SettlementDate.Year
            })
            .OrderBy(transferGroup => transferGroup.Key.Year)
            .ThenBy(transferGroup => transferGroup.Key.Month)
            .Select(transferGroup => new MonthlyBalanceDataResponse
            {
                Year = transferGroup.Key.Year,
                Month = transferGroup.Key.Month,
                Income = transferGroup.Sum(transfer => transfer.Type == TransferType.Profit ? transfer.Value : 0),
                Outcome = transferGroup.Sum(transfer => transfer.Type == TransferType.Expense ? transfer.Value : 0),
                Reference = new DateTime(transferGroup.Key.Year, transferGroup.Key.Month, 1)
                    .ToString("MMM/yy", CultureInfo.InvariantCulture)
            })
            .ToListAsync(cancellationToken);

        var discriminatedBalanceData = monthsInRange
            .GroupJoin(
                groupedTransfers,
                month => new { month.Year, month.Month },
                transfer => new { transfer.Year, transfer.Month },
                (monthlyBalanceData, transfers) => new MonthlyBalanceDataResponse
                {
                    Year = monthlyBalanceData.Year,
                    Month = monthlyBalanceData.Month,
                    Income = transfers.Sum(transfer => transfer.Income),
                    Outcome = transfers.Sum(transfer => transfer.Outcome),
                    Reference = new DateTime(monthlyBalanceData.Year, monthlyBalanceData.Month, 1)
                        .ToString("MMM/yy", CultureInfo.InvariantCulture)
                })
            .ToList()
            .AsReadOnly();

        return Result.Ok(new DiscriminatedBalanceDataResponse
        {
            DiscriminatedBalanceData = discriminatedBalanceData.AsReadOnly()
        });
    }
}
