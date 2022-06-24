using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Transfers.Commands.DeleteTransfer
{
    internal sealed class DeleteTransferHandler : IRequestHandler<DeleteTransferCommand>
    {
        private readonly ILogger<DeleteTransferHandler> _logger;
        private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public DeleteTransferHandler(
            ILogger<DeleteTransferHandler> logger, 
            IMonthlyBalanceRepository monthlyBalanceRepository, 
            IBusinessUnitRepository businessUnitRepository)
        {
            _logger = logger;
            _monthlyBalanceRepository = monthlyBalanceRepository;
            _businessUnitRepository = businessUnitRepository;
        }

        public async Task<Unit> Handle(DeleteTransferCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Removing Transfer with Id {TransferId}", command.TransferId);
            var businessUnit = await _businessUnitRepository.GetByIdAsync(command.BusinessUnitId, cancellationToken);
            var monthlyBalance = await _monthlyBalanceRepository.GetByIdAsync(command.MonhlyBalanceId, cancellationToken);
            
            var transferValue = monthlyBalance.GetTransferById(command.TransferId).FormattedValue;
            monthlyBalance.DeleteTransferById(command.TransferId);

            _logger.LogInformation("Updating Balance of Business Unit with Id {BusinessUnitId}", command.BusinessUnitId);
            businessUnit.AddBalance(-transferValue);
            _businessUnitRepository.Update(businessUnit);

            _logger.LogInformation("Updating Monthly Balance with Id {MonthlyBalanceId}", command.MonhlyBalanceId);
            _monthlyBalanceRepository.Update(monthlyBalance);

            _logger.LogInformation("Transfer with Id {TransferId} sucessfully removed", command.TransferId);
            return Unit.Value;
        }
    }
}
