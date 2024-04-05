using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetTransfers;

internal sealed class GetTransfersHandler(ITransferRepository transferRepository)
    : IQueryHandler<GetTransfersQuery, Paginated<TransferGroupResponse>>
{
    private readonly ITransferRepository _transferRepository = transferRepository;

    public async Task<Result<Paginated<TransferGroupResponse>>> Handle(GetTransfersQuery query, CancellationToken cancellationToken)
    {
        var (totalCount, transfers) = await _transferRepository.GetTransfersByParams(
            query.BusinessUnitId,
            query.StartDate,
            query.EndDate,
            query.CategoryId,
            query.AccountTagId,
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        return TransferMapper.DTR.Map(transfers, query.PageNumber, query.PageSize, totalCount);
    }
}
