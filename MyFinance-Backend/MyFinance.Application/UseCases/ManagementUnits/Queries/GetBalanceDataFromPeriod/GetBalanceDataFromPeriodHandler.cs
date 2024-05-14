using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.ManagementUnit.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetBalanceDataFromPeriod;

internal sealed class GetPeriodBalanceHandler(ITransferRepository transferRepository)
    : IQueryHandler<GetBalanceDataFromPeriodQuery, PeriodBalanceDataResponse>
{
    private readonly ITransferRepository _transferRepository = transferRepository;

    public async Task<Result<PeriodBalanceDataResponse>> Handle(GetBalanceDataFromPeriodQuery query, CancellationToken cancellationToken)
    {
        var periodBalanceData = await _transferRepository.GetBalanceDataFromPeriodAsync(
            query.ManagementUnitId,
            query.StartDate,
            query.EndDate,
            query.CategoryId,
            query.AccountTagId,
            cancellationToken);

        return Result.Ok(ManagementUnitMapper.DTR.Map(periodBalanceData));
    }
}
