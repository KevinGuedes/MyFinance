using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetAnnualBalanceData;

internal sealed class GetAnnualBalanceDataHandler(ITransferRepository transferRepository) 
    : IQueryHandler<GetAnnualBalanceDataQuery, AnnualBalanceDataResponse>
{
    private readonly ITransferRepository _transferRepository = transferRepository;

    public async Task<Result<AnnualBalanceDataResponse>> Handle(GetAnnualBalanceDataQuery query, CancellationToken cancellationToken)
    {
        var annualBalanceData = await _transferRepository
            .GetAnnualBalanceDataAsync(query.BusinessUnitId, query.Year, cancellationToken);

        return Result.Ok(TransferMapper.DTR.Map(query.Year, annualBalanceData));
    }
}
