using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.BusinessUnits.Commands.DeleteBusinessUnitById
{
    internal sealed class DeleteBusinessUnitByIdHandler : IRequestHandler<DeleteBusinessUnitByIdCommand>
    {
        private readonly ILogger<DeleteBusinessUnitByIdHandler> _logger;
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public DeleteBusinessUnitByIdHandler(
            ILogger<DeleteBusinessUnitByIdHandler> logger,
            IBusinessUnitRepository businessUnitRepository)
            => (_logger, _businessUnitRepository) = (logger, businessUnitRepository);

        public Task<Unit> Handle(DeleteBusinessUnitByIdCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Removing Business Unit with id {BusinessUnitId}", command.BusinessUnitId);
            _businessUnitRepository.DeleteById(command.BusinessUnitId);
            _logger.LogInformation("Business Unit removed");

            return Task.FromResult(Unit.Value);
        }
    }
}
