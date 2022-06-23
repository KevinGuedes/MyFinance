using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.BusinessUnits.Commands.RemoveBusinessUnitById
{
    internal sealed class RemoveBusinessUnitByIdCommandHandler : IRequestHandler<RemoveBusinessUnitByIdCommand>
    {
        private readonly ILogger<RemoveBusinessUnitByIdCommandHandler> _logger;
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public RemoveBusinessUnitByIdCommandHandler(
            ILogger<RemoveBusinessUnitByIdCommandHandler> logger,
            IBusinessUnitRepository businessUnitRepository)
            => (_logger, _businessUnitRepository) = (logger, businessUnitRepository);

        public Task<Unit> Handle(RemoveBusinessUnitByIdCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Removing Business Unit with id {BusinessUnitId}",request.BusinessUnitId);
            _businessUnitRepository.RemoveById(request.BusinessUnitId);
            _logger.LogInformation("Business Unit removed");

            return Task.FromResult(Unit.Value);
        }
    }
}
