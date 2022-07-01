using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Generics.Requests;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.MonthlyBalances.Queries.GetRecentMonthlyBalances
{
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
            _logger.LogInformation("Retrieving top {Count} recent Monthly Balances", query.Count);
            var topRecentMonthlybalances = await _monthlyBalanceRepository.GetAllAsync(query.Count, query.Skip, cancellationToken);
            _logger.LogInformation("Top {Count} recent Monthly Balances retrieved", query.Count);

            return Result.Ok(topRecentMonthlybalances);
        }
    }
}
