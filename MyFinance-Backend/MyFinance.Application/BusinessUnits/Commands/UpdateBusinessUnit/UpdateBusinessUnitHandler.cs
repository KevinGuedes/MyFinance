using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Generics.Requests;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.BusinessUnits.Commands.UpdateBusinessUnit
{
    internal sealed class UpdateBusinessUnitHandler : CommandHandler<UpdateBusinessUnitCommand, BusinessUnit>
    {
        private readonly ILogger<UpdateBusinessUnitHandler> _logger;
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public UpdateBusinessUnitHandler(
            ILogger<UpdateBusinessUnitHandler> logger,
            IBusinessUnitRepository businessUnitRepository)
            => (_logger, _businessUnitRepository) = (logger, businessUnitRepository);

        public async override Task<Result<BusinessUnit>> Handle(UpdateBusinessUnitCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving Business Unit with Id {} from databse", command.BusinessUnitId);
            var businessUnit = await _businessUnitRepository.GetByIdAsync(command.BusinessUnitId, cancellationToken);

            _logger.LogInformation("Updating Business Unit with id {BusinessUnitId}", command.BusinessUnitId);
            businessUnit.Update(command.Name, command.IsArchived);
            _businessUnitRepository.Update(businessUnit);
            _logger.LogInformation("Business Unit with id {BusinessUnitId} updated", command.BusinessUnitId);

            return Result.Ok(businessUnit);
        }
    }
}
