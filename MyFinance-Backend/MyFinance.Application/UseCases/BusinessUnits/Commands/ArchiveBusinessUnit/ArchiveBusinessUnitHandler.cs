using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit
{
    internal sealed class ArchiveBusinessUnitHandler : ICommandHandler<ArchiveBusinessUnitCommand>
    {
        private readonly ILogger<ArchiveBusinessUnitHandler> _logger;
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public ArchiveBusinessUnitHandler(
            ILogger<ArchiveBusinessUnitHandler> logger, 
            IBusinessUnitRepository businessUnitRepository)
            => (_logger, _businessUnitRepository) = (logger, businessUnitRepository);

        public async Task<Result> Handle(ArchiveBusinessUnitCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving Business Unit with Id {BusinessUnitId}", command.Id);
            var businessUnit = await _businessUnitRepository.GetByIdAsync(command.Id, cancellationToken);

            _logger.LogInformation("Archiving Business Unit with Id {BusinessUnitId}", command.Id);
            businessUnit!.Archive(command.ReasonToArchive);
            _businessUnitRepository.Update(businessUnit);

            _logger.LogInformation("Business Unit with Id {BusinessUnitId} successfuly archived", command.Id);
            return Result.Ok();
        }
    }
}
