using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.AccountTag.Responses;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.MonthlyBalance.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.MonthlyBalances.Queries.GetMonthlyBalances;

internal sealed class GetMonthlyBalancesHandler(
    IBusinessUnitRepository businessUnitRepository,
    IMonthlyBalanceRepository monthlyBalanceRepository,
    ICurrentUserProvider currentUserProvider) 
    : IQueryHandler<GetMonthlyBalancesQuery, PaginatedResponse<MonthlyBalanceResponse>>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository = monthlyBalanceRepository;

    public async Task<Result<PaginatedResponse<MonthlyBalanceResponse>>> Handle(GetMonthlyBalancesQuery query,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        var isValidBusinessUnit = await _businessUnitRepository.ExistsByIdAsync(
            query.BusinessUnitId,
            currentUserId,
            cancellationToken);

        if (!isValidBusinessUnit)
        {
            var errorMessage = $"Business Unit with Id {query.BusinessUnitId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var monthlyBalances = await _monthlyBalanceRepository.GetPaginatedByBusinessUnitIdAsync(
            query.BusinessUnitId,
            query.PageNumber,
            query.PageSize,
            currentUserId,
            cancellationToken);


        var response = new PaginatedResponse<MonthlyBalanceResponse>(
            MonthlyBalanceMapper.DTR.Map(monthlyBalances),
            query.PageNumber,
            query.PageSize,
            0);

        return Result.Ok(response);
    }
}