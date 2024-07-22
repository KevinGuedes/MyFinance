using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.ManagementUnit.Responses;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetManagementUnit;

internal sealed class GetManagementUnitHandler(IManagementUnitRepository managementUnitRepository)
    : IQueryHandler<GetManagementUnitQuery, ManagementUnitResponse>
{
    private readonly IManagementUnitRepository _managementUnitRepository = managementUnitRepository;

    public async Task<Result<ManagementUnitResponse>> Handle(GetManagementUnitQuery query,
        CancellationToken cancellationToken)
    {
        var managementUnit = await _managementUnitRepository.GetByIdAsync(query.Id, cancellationToken);

        if (managementUnit is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Management Unit with Id {query.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        return Result.Ok(ManagementUnitMapper.DTR.Map(managementUnit));
    }
}