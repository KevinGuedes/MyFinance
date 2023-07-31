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

    public GetMonthlyBalancesHandler(
        ILogger<GetMonthlyBalancesHandler> logger,
        IBusinessUnitRepository businessUnitRepository)
        => (_logger, _businessUnitRepository) = (logger, businessUnitRepository);

    public async Task<Result<IEnumerable<MonthlyBalance>>> Handle(GetMonthlyBalancesQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving Business Unit with Id {BusinessUnitId} to retrieve its Monthly Balances", query.BusinessUnitId);
        var businessUnit = await _businessUnitRepository.GetWithMonthlyBalancesPaginated(
            query.BusinessUnitId,
            query.Page,
            query.PageSize,
            cancellationToken);

        if (businessUnit is null)
        {
            _logger.LogWarning("Business Unit with Id {BusinessUnitId} not found", query.BusinessUnitId);
            var errorMessage = string.Format("Business Unit with Id {0} not found", query.BusinessUnitId);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        _logger.LogInformation("Monthly Balances retrieved");
        return Result.Ok(businessUnit.MonthlyBalances.AsEnumerable());
    }
}
