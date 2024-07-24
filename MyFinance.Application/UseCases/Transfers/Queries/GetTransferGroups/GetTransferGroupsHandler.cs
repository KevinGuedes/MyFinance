using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetTransferGroups;

internal sealed class GetTransferGroupsHandler(ITransferRepository transferRepository)
    : IQueryHandler<GetTransferGroupsQuery, Paginated<TransferGroupResponse>>
{
    private readonly ITransferRepository _transferRepository = transferRepository;

    public async Task<Result<Paginated<TransferGroupResponse>>> Handle(GetTransferGroupsQuery query, CancellationToken cancellationToken)
    {
        var transfersData = await _transferRepository.GetTransferGroupsAsync(
            query.Month,
            query.Year,
            query.ManagementUnitId,
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        return TransferMapper.DTR.Map(transfersData, query.PageNumber, query.PageSize);
    }
}
