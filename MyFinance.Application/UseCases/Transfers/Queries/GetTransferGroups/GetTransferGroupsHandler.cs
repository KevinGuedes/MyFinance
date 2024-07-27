using FluentResults;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Helpers;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetTransferGroups;

internal sealed class GetTransferGroupsHandler(IMyFinanceDbContext myFinanceDbContext)
    : IQueryHandler<GetTransferGroupsQuery, Paginated<TransferGroupResponse>>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<Paginated<TransferGroupResponse>>> Handle(GetTransferGroupsQuery query, CancellationToken cancellationToken)
    {
        var startDate = MonthHelper.GetFirstDateOfMonthInUTC(query.Month, query.Year);
        var endDate = MonthHelper.GetLastDateOfMonthInUTC(query.Month, query.Year);

        var transfers = _myFinanceDbContext.Transfers
           .AsNoTracking()
           .Where(transfer =>
                transfer.ManagementUnitId == query.ManagementUnitId &&
                transfer.SettlementDate >= startDate &&
                transfer.SettlementDate <= endDate);

        var totalCount = await transfers.LongCountAsync(cancellationToken);

        var transferGroups = await transfers
            .Include(transfer => transfer.Category.Name)
            .Include(transfer => transfer.AccountTag.Tag)
            .OrderByDescending(transfer => transfer.SettlementDate)
            .ThenBy(transfer => transfer.RelatedTo)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .GroupBy(transfer => transfer.SettlementDate.Day)
            .Select(transferGroup => new TransferGroupResponse
            {
                Date = new DateOnly(query.Year, query.Month, transferGroup.Key),
                Income = transferGroup.Sum(transfer => transfer.Type == TransferType.Profit ? transfer.Value : 0),
                Outcome = transferGroup.Sum(transfer => transfer.Type == TransferType.Expense ? transfer.Value : 0),
                Transfers = transferGroup
                    .OrderByDescending(transfer => transfer.SettlementDate)
                    .ThenBy(transfer => transfer.RelatedTo)
                    .Select(transfer => new TransferResponse
                    {
                        Id = transfer.Id,
                        RelatedTo = transfer.RelatedTo,
                        Value = transfer.Value,
                        Description = transfer.Description,
                        SettlementDate = transfer.SettlementDate,
                        Type = transfer.Type,
                        CategoryName = transfer.Category.Name,
                        Tag = transfer.AccountTag.Tag,
                    })
                    .ToList()
                    .AsReadOnly()
            })
            .ToListAsync(cancellationToken);

        return Result.Ok(new Paginated<TransferGroupResponse>
        {
            Items = transferGroups.AsReadOnly(),
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        });
    }
}
