using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.BusinessUnits.Commands.UpdateBusinessUnit
{
    internal sealed class UpdateBusinessUnitHandler : IRequestHandler<UpdateBusinessUnitCommand, BusinessUnit>
    {
        private readonly ILogger<UpdateBusinessUnitHandler> _logger;
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public UpdateBusinessUnitHandler(
            ILogger<UpdateBusinessUnitHandler> logger,
            IBusinessUnitRepository businessUnitRepository)
            => (_logger, _businessUnitRepository) = (logger, businessUnitRepository);

        public async Task<BusinessUnit> Handle(UpdateBusinessUnitCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving Business Unit with Id {} from databse", command.BusinessUnitId);
            var businessUnit = await _businessUnitRepository.GetByIdAsync(command.BusinessUnitId, cancellationToken);

            _logger.LogInformation("Updating Business Unit with id {BusinessUnitId}", command.BusinessUnitId);
            businessUnit.Update(command.Name, command.IsArchived);
            _businessUnitRepository.Update(businessUnit);
            _logger.LogInformation("Business Unit with id {BusinessUnitId} updated", command.BusinessUnitId);

            return businessUnit;
        }
    }
}
