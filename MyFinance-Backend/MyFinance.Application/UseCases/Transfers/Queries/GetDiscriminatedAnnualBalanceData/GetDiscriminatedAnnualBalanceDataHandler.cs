using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetDiscriminatedAnnualBalanceData;

internal sealed class GetDiscriminatedAnnualBalanceDataHandler(ITransferRepository transferRepository) 
    : IQueryHandler<GetDiscriminatedAnnualBalanceDataQuery, DiscriminatedAnnualBalanceDataResponse>
{
    private readonly ITransferRepository _transferRepository = transferRepository;

    public async Task<Result<DiscriminatedAnnualBalanceDataResponse>> Handle(GetDiscriminatedAnnualBalanceDataQuery query, CancellationToken cancellationToken)
    {
        var discriminatedAnnualBalanceData = await _transferRepository
            .GetDiscriminatedAnnualBalanceDataAsync(query.BusinessUnitId, query.Year, cancellationToken);

        return Result.Ok(TransferMapper.DTR.Map(query.Year, discriminatedAnnualBalanceData));
    }
}
