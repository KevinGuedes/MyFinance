using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetTransferGroups;

public sealed record GetTransferGroupsQuery(
    int Month,
    int Year,
    Guid ManagementUnitId,
    int PageNumber,
    int PageSize) : IQuery<Paginated<TransferGroupResponse>>
{
}
