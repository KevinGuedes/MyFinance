using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetBalanceDataFromPeriod;

internal sealed class GetPeriodBalanceHandler(ITransferRepository transferRepository)
    : IQueryHandler<GetBalanceDataFromPeriodQuery, PeriodBalanceDataResponse>
{
    private readonly ITransferRepository _transferRepository = transferRepository;

    public async Task<Result<PeriodBalanceDataResponse>> Handle(GetBalanceDataFromPeriodQuery query, CancellationToken cancellationToken)
    {
        var periodBalanceData = await _transferRepository.GetBalanceDataFromPeriodAsync(
            query.BusinessUnitId,
            query.StartDate,
            query.EndDate,
            query.CategoryId,
            query.AccountTagId,
            cancellationToken);

        return Result.Ok(TransferMapper.DTR.Map(periodBalanceData));
    }
}
