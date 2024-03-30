using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetTransfers;

public sealed record GetTransfersQuery(
    Guid BusinessUnitId,
    DateOnly? From,
    DateOnly? To,
    Guid? CategoryId,
    Guid? AccountTagId,
    int PageNumber, 
    int PageSize) : IQuery<Paginated<TransferGroupResponse>>
{
}
