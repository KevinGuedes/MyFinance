﻿using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.MonthlyBalances.Queries.GetMonthlyBalances;

internal sealed class GetMonthlyBalancesHandler(
    ILogger<GetMonthlyBalancesHandler> logger,
    IBusinessUnitRepository businessUnitRepository,
    IMonthlyBalanceRepository monthlyBalanceRepository,
    ICurrentUserProvider currentUserProvider) : IQueryHandler<GetMonthlyBalancesQuery, IEnumerable<MonthlyBalance>>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;
    private readonly ILogger<GetMonthlyBalancesHandler> _logger = logger;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository = monthlyBalanceRepository;

    public async Task<Result<IEnumerable<MonthlyBalance>>> Handle(GetMonthlyBalancesQuery query,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        _logger.LogInformation("Retrieving Business Unit with Id {BusinessUnitId} to retrieve its Monthly Balances",
            query.BusinessUnitId);
        var isValidBusinessUnit = await _businessUnitRepository.ExistsByIdAsync(
            query.BusinessUnitId,
            currentUserId,
            cancellationToken);

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
            currentUserId,
            cancellationToken);

        _logger.LogInformation("Monthly Balances retrieved");
        return Result.Ok(monthlyBalances);
    }
}