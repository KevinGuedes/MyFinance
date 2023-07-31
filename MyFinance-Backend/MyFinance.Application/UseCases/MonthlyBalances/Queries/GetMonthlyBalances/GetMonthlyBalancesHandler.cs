using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.MonthlyBalances.Queries.GetMonthlyBalances;

internal sealed class GetMonthlyBalancesHandler : IQueryHandler<GetMonthlyBalancesQuery, IEnumerable<MonthlyBalance>>
{
    private readonly ILogger<GetMonthlyBalancesHandler> _logger;
    private readonly IBusinessUnitRepository _businessUnitRepository;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;

    public GetMonthlyBalancesHandler(
        ILogger<GetMonthlyBalancesHandler> logger,
        IBusinessUnitRepository businessUnitRepository,
        IMonthlyBalanceRepository monthlyBalanceRepository)
        => (_logger, _businessUnitRepository, _monthlyBalanceRepository) = (logger, businessUnitRepository, monthlyBalanceRepository);

    public async Task<Result<IEnumerable<MonthlyBalance>>> Handle(GetMonthlyBalancesQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving Business Unit with Id {BusinessUnitId} to retrieve its Monthly Balances", query.BusinessUnitId);
        var isValidBusinessUnit = await _businessUnitRepository.ExistsByIdAsync(query.BusinessUnitId, cancellationToken);

        if (!isValidBusinessUnit)
        {
            _logger.LogWarning("Business Unit with Id {BusinessUnitId} not found", query.BusinessUnitId);
            var errorMessage = string.Format("Business Unit with Id {0} not found", query.BusinessUnitId);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var monthlyBalances = await _monthlyBalanceRepository.GetPaginatedByBusinessUnitIdAsync(
            query.BusinessUnitId,
            query.Page,
            query.PageSize,
            cancellationToken);

        _logger.LogInformation("Monthly Balances retrieved");
        return Result.Ok(monthlyBalances);
    }
}
