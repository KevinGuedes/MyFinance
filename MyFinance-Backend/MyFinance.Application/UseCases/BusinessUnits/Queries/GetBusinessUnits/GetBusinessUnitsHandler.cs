using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.BusinessUnits.Queries.GetBusinessUnits;

internal sealed class GetBusinessUnitsHandler(
    ILogger<GetBusinessUnitsHandler> logger,
    IBusinessUnitRepository businessUnitRepository) : IQueryHandler<GetBusinessUnitsQuery, IEnumerable<BusinessUnit>>
{
    private readonly ILogger<GetBusinessUnitsHandler> _logger = logger;
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;

    public async Task<Result<IEnumerable<BusinessUnit>>> Handle(GetBusinessUnitsQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving Business Units from database");
        var businessUnits = await _businessUnitRepository.GetPaginatedAsync(query.Page, query.PageSize, cancellationToken);
        _logger.LogInformation("Business Units successfully retrived from database");

        return Result.Ok(businessUnits);
    }
}
