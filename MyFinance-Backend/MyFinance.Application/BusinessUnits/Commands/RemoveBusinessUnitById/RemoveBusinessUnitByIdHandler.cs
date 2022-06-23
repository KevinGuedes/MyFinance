using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.BusinessUnits.Commands.RemoveBusinessUnitById
{
    internal sealed class RemoveBusinessUnitByIdHandler : IRequestHandler<RemoveBusinessUnitByIdCommand>
    {
        private readonly ILogger<RemoveBusinessUnitByIdHandler> _logger;
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public RemoveBusinessUnitByIdHandler(
            ILogger<RemoveBusinessUnitByIdHandler> logger,
            IBusinessUnitRepository businessUnitRepository)
            => (_logger, _businessUnitRepository) = (logger, businessUnitRepository);

        public Task<Unit> Handle(RemoveBusinessUnitByIdCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Removing Business Unit with id {BusinessUnitId}", command.BusinessUnitId);
            _businessUnitRepository.RemoveById(command.BusinessUnitId);
            _logger.LogInformation("Business Unit removed");

            return Task.FromResult(Unit.Value);
        }
    }
}
