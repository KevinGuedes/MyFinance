using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Generics.Requests;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Transfers.Commands.DeleteTransfer
{
    internal sealed class DeleteTransferHandler : CommandHandler<DeleteTransferCommand>
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

        public async override Task<Result> Handle(DeleteTransferCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting Transfer with Id {TransferId}", command.TransferId);
            var monthlyBalance = await _monthlyBalanceRepository.GetByIdAsync(command.MonthlyBalanceId, cancellationToken);
            var businessUnit = await _businessUnitRepository.GetByIdAsync(monthlyBalance.ReferenceData.BusinessUnitId, cancellationToken);
            var transfer = monthlyBalance.PopTransferById(command.TransferId);

            _logger.LogInformation("Updating Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance.Id);
            _monthlyBalanceRepository.Update(monthlyBalance);

            _logger.LogInformation("Updating Balance of Business Unit with Id {BusinessUnitId}", businessUnit.Id);
            businessUnit.AddBalance(-transfer.Value);
            _businessUnitRepository.Update(businessUnit);

            _logger.LogInformation("Transfer with Id {TransferId} sucessfully deleted", transfer.Id);
            return Result.Ok();
        }
    }
}
