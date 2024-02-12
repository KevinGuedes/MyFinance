using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Application.Services.CurrentUserProvider;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit
{
    internal sealed class ArchiveBusinessUnitHandler(
        ILogger<ArchiveBusinessUnitHandler> logger,
        IBusinessUnitRepository businessUnitRepository,
        ICurrentUserProvider currentUserProvider) : ICommandHandler<ArchiveBusinessUnitCommand>
    {
        private readonly ILogger<ArchiveBusinessUnitHandler> _logger = logger;
        private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
        private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

        public async Task<Result> Handle(ArchiveBusinessUnitCommand command, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserProvider.GetCurrentUserId();

            _logger.LogInformation("Retrieving Business Unit with Id {BusinessUnitId}", command.Id);
            var businessUnit = await _businessUnitRepository.GetByIdAsync(command.Id, currentUserId, cancellationToken);

            if (businessUnit is null)
            {
                _logger.LogWarning("Business Unit with Id {BusinessUnitId} not found", command.Id);
                var errorMessage = string.Format("Business Unit with Id {0} not found", command.Id);
                var entityNotFoundError = new EntityNotFoundError(errorMessage);
                return Result.Fail(entityNotFoundError);
            }

            _logger.LogInformation("Archiving Business Unit with Id {BusinessUnitId}", command.Id);
            businessUnit.Archive(command.ReasonToArchive);
            _businessUnitRepository.Update(businessUnit);

            _logger.LogInformation("Business Unit with Id {BusinessUnitId} successfuly archived", command.Id);
            return Result.Ok();
        }
    }
}
