using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.BusinessUnit.Responses;
using MyFinance.Contracts.Common;

namespace MyFinance.Application.UseCases.BusinessUnits.Queries.GetBusinessUnits;

internal sealed class GetBusinessUnitsHandler(IBusinessUnitRepository businessUnitRepository, ICurrentUserProvider currentUserProvider)
    : IQueryHandler<GetBusinessUnitsQuery, PaginatedResponse<BusinessUnitResponse>>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<Result<PaginatedResponse<BusinessUnitResponse>>> Handle(GetBusinessUnitsQuery query,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        var businessUnits = await _businessUnitRepository.GetPaginatedAsync(
            query.PageNumber,
            query.PageSize,
            currentUserId,
            cancellationToken);

        var response = new PaginatedResponse<BusinessUnitResponse>(
            BusinessUnitMapper.DTR.Map(businessUnits),
            query.PageNumber,
            query.PageSize,
            0);

        return Result.Ok(response);
    }
}