using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.BusinessUnits.Queries.GetBusinessUnits;

internal sealed class GetBusinessUnitsHandler(
    ILogger<GetBusinessUnitsHandler> logger,
    IBusinessUnitRepository businessUnitRepository,
    ICurrentUserProvider currentUserProvider) : IQueryHandler<GetBusinessUnitsQuery, IEnumerable<BusinessUnit>>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;
    private readonly ILogger<GetBusinessUnitsHandler> _logger = logger;

    public async Task<Result<IEnumerable<BusinessUnit>>> Handle(GetBusinessUnitsQuery query,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        _logger.LogInformation("Retrieving Business Units from database");
        var businessUnits = await _businessUnitRepository.GetPaginatedAsync(
            query.Page,
            query.PageSize,
            currentUserId,
            cancellationToken);
        _logger.LogInformation("Business Units successfully retrived from database");

        return Result.Ok(businessUnits);
    }
}