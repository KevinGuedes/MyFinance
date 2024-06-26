using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Helpers;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetDiscriminatedBalanceData;

internal sealed class GetDiscriminatedBalanceDataHandler(ITransferRepository transferRepository)
    : IQueryHandler<GetDiscriminatedBalanceDataQuery, DiscriminatedBalanceDataResponse>
{
    private readonly ITransferRepository _transferRepository = transferRepository;

    public async Task<Result<DiscriminatedBalanceDataResponse>> Handle(
        GetDiscriminatedBalanceDataQuery query,
        CancellationToken cancellationToken)
    {
        var (fromDate, toDate) = PastMonthsHelper.GetDateRangeFromPastMonths(
            query.PastMonths, 
            query.IncludeCurrentMonth);

        var discriminatedBalanceData = await _transferRepository
            .GetDiscriminatedBalanceDataAsync(query.ManagementUnitId, fromDate, toDate, cancellationToken);

        return Result.Ok(TransferMapper.DTR.Map(discriminatedBalanceData, fromDate, toDate));
    }
}
