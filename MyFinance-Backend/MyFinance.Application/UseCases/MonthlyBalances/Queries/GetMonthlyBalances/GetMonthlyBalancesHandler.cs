using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.RequestHandling;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.MonthlyBalances.Queries.GetMonthlyBalances;

internal sealed class GetMonthlyBalancesHandler : QueryHandler<GetMonthlyBalancesQuery, IEnumerable<MonthlyBalance>>
{
    private readonly ILogger<GetMonthlyBalancesHandler> _logger;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;

    public GetMonthlyBalancesHandler(
        ILogger<GetMonthlyBalancesHandler> logger,
        IMonthlyBalanceRepository monthlyBalanceRepository)
        => (_logger, _monthlyBalanceRepository) = (logger, monthlyBalanceRepository);

    public async override Task<Result<IEnumerable<MonthlyBalance>>> Handle(GetMonthlyBalancesQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving Monthly Balances");
        var topRecentMonthlybalances = await _monthlyBalanceRepository.GetByBusinessUnitId(
            query.BusinessUnitId,
            query.Take,
            query.Skip,
            cancellationToken);
        _logger.LogInformation("Monthly Balances retrieved");

        return Result.Ok(topRecentMonthlybalances);
    }
}
