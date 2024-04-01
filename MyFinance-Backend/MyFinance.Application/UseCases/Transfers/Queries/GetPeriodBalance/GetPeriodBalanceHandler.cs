using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Transfer.Responses;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetPeriodBalance;

internal sealed class GetPeriodBalanceHandler(ITransferRepository transferRepository)
    : IQueryHandler<GetPeriodBalanceQuery, PeriodBalanceResponse>
{
    private readonly ITransferRepository _transferRepository = transferRepository;

    public async Task<Result<PeriodBalanceResponse>> Handle(GetPeriodBalanceQuery query, CancellationToken cancellationToken)
    {
        var (income, outcome) = await _transferRepository.GetIncomeAndOutcomeFromPeriodByParams(
            query.BusinessUnitId,
            query.StartDate,
            query.EndDate,
            query.CategoryId,
            query.AccountTagId,
            cancellationToken);

        return Result.Ok(TransferMapper.DTR.Map(income, outcome));
    }
}
