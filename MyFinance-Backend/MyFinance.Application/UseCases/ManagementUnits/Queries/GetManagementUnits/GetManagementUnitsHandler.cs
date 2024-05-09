using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.ManagementUnit.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetManagementUnits;

internal sealed class GetManagementUnitsHandler(IManagementUnitRepository managementUnitRepository)
    : IQueryHandler<GetManagementUnitsQuery, Paginated<ManagementUnitResponse>>
{
    private readonly IManagementUnitRepository _managementUnitRepository = managementUnitRepository;

    public async Task<Result<Paginated<ManagementUnitResponse>>> Handle(GetManagementUnitsQuery query,
        CancellationToken cancellationToken)
    {
        var totalCount = await _managementUnitRepository.GetTotalCountAsync(cancellationToken);

        var managementUnits = await _managementUnitRepository.GetPaginatedAsync(
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        var response = ManagementUnitMapper.DTR.Map(
            managementUnits,
            query.PageNumber,
            query.PageSize,
            totalCount);

        return Result.Ok(response);
    }
}