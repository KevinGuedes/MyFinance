using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.BusinessUnits.Queries.GetBusinessUnits
{
    internal class GetBusinessUnitsQueryHandler : IRequestHandler<GetBusinessUnitsQuery, IEnumerable<BusinessUnit>>
    {
        private readonly ILogger<GetBusinessUnitsQueryHandler> _logger;
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public GetBusinessUnitsQueryHandler(
            ILogger<GetBusinessUnitsQueryHandler> logger,
            IBusinessUnitRepository businessUnitRepository)
            => (_logger, _businessUnitRepository) = (logger, businessUnitRepository);

        public async Task<IEnumerable<BusinessUnit>> Handle(GetBusinessUnitsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving Business Units from database");
            var businessUnits = await _businessUnitRepository.GetAllAsync(cancellationToken);
            _logger.LogInformation("Business Units successfully retrived from database");

            return businessUnits;
        }
    }
}
