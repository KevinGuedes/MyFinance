using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit
{
    internal sealed class CreateBusinessUnitCommandHandler : IRequestHandler<CreateBusinessUnitCommand, BusinessUnit>
    {
        private readonly ILogger<CreateBusinessUnitCommandHandler> _logger;
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public CreateBusinessUnitCommandHandler(
            ILogger<CreateBusinessUnitCommandHandler> logger,
            IBusinessUnitRepository businessUnitRepository)
            => (_logger, _businessUnitRepository) = (logger, businessUnitRepository);

        public Task<BusinessUnit> Handle(CreateBusinessUnitCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating new Business Unit with Name: {Name}", request.Name);
            var businessUnit = new BusinessUnit(request.Name);
            _businessUnitRepository.Insert(businessUnit);
            _logger.LogInformation("Business Unit successfully created");

            return Task.FromResult(businessUnit);
        }
    }
}
