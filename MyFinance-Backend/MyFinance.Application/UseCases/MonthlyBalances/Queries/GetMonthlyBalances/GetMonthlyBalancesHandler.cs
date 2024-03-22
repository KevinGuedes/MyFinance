using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.MonthlyBalance.Responses;

namespace MyFinance.Application.UseCases.MonthlyBalances.Queries.GetMonthlyBalances;

internal sealed class GetMonthlyBalancesHandler(
    IBusinessUnitRepository businessUnitRepository,
    IMonthlyBalanceRepository monthlyBalanceRepository)
    : IQueryHandler<GetMonthlyBalancesQuery, Paginated<MonthlyBalanceResponse>>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository = monthlyBalanceRepository;

    public async Task<Result<Paginated<MonthlyBalanceResponse>>> Handle(GetMonthlyBalancesQuery query,
        CancellationToken cancellationToken)
    {
        var isValidBusinessUnit = await _businessUnitRepository.ExistsByIdAsync(
            query.BusinessUnitId,
            cancellationToken);

        if (!isValidBusinessUnit)
        {
            var entityNotFoundError =
                new EntityNotFoundError($"Business Unit with Id {query.BusinessUnitId} not found");
            return Result.Fail(entityNotFoundError);
        }

        var monthlyBalances = await _monthlyBalanceRepository.GetPaginatedByBusinessUnitIdAsync(
            query.BusinessUnitId,
            query.PageNumber,
            query.PageSize,
            cancellationToken);


        var response = new Paginated<MonthlyBalanceResponse>(
            MonthlyBalanceMapper.DTR.Map(monthlyBalances),
            query.PageNumber,
            query.PageSize,
            0);

        return Result.Ok(response);
    }
}