using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Generics.Requests;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.BusinessUnits.Queries.GetBusinessUnits
{
    internal sealed class GetBusinessUnitsHandler : QueryHandler<GetBusinessUnitsQuery, IEnumerable<BusinessUnit>>
    {
        private readonly ILogger<GetBusinessUnitsHandler> _logger;
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public GetBusinessUnitsHandler(
            ILogger<GetBusinessUnitsHandler> logger,
            IBusinessUnitRepository businessUnitRepository)
            => (_logger, _businessUnitRepository) = (logger, businessUnitRepository);

        public async override Task<Result<IEnumerable<BusinessUnit>>> Handle(GetBusinessUnitsQuery query, CancellationToken cancellationToken)
        {
            throw new Exception("Deu ruim");
            _logger.LogInformation("Retrieving Business Units from database");
            var businessUnits = await _businessUnitRepository.GetAllAsync(cancellationToken);
            _logger.LogInformation("Business Units successfully retrived from database");

            return Result.Ok(businessUnits);
        }
    }
}
