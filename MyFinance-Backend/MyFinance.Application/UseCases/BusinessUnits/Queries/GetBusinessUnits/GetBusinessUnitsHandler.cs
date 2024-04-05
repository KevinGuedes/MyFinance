using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.BusinessUnit.Responses;
using MyFinance.Contracts.Common;

namespace MyFinance.Application.UseCases.BusinessUnits.Queries.GetBusinessUnits;

internal sealed class GetBusinessUnitsHandler(IBusinessUnitRepository businessUnitRepository)
    : IQueryHandler<GetBusinessUnitsQuery, Paginated<BusinessUnitResponse>>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;

    public async Task<Result<Paginated<BusinessUnitResponse>>> Handle(GetBusinessUnitsQuery query,
        CancellationToken cancellationToken)
    {
        var totalCount = await _businessUnitRepository.GetTotalCountAsync(cancellationToken);

        var businessUnits = await _businessUnitRepository.GetPaginatedAsync(
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        var response = BusinessUnitMapper.DTR.Map(
            businessUnits, 
            query.PageNumber, 
            query.PageSize,
            totalCount);

        return Result.Ok(response);
    }
}