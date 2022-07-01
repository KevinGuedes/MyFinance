using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Generics.Requests;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Transfers.Commands.UpdateTransfer
{
    internal sealed class UpdateTransferHandler : CommandHandler<UpdateTransferCommand, Transfer>
    {
        private readonly ILogger<UpdateTransferHandler> _logger;
        private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public UpdateTransferHandler(
            ILogger<UpdateTransferHandler> logger,
            IMonthlyBalanceRepository monthlyBalanceRepository,
            IBusinessUnitRepository businessUnitRepository)
        {
            _logger = logger;
            _monthlyBalanceRepository = monthlyBalanceRepository;
            _businessUnitRepository = businessUnitRepository;
        }

        public async override Task<Result<Transfer>> Handle(UpdateTransferCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving previous Monthly Balance with Id {PreviousMonthlyBalanceId}", command.CurrentMonthlyBalanceId);
            var currentMonthlyBalance = await _monthlyBalanceRepository.GetByIdAsync(command.CurrentMonthlyBalanceId, cancellationToken);
            var businessUnitId = currentMonthlyBalance.BusinessUnitId;
            var transfer = currentMonthlyBalance.GetTransferById(command.TransferId);

            await UpdateBusinessUnitBalanceIfNeeded(command.Value, businessUnitId, transfer.Value, cancellationToken);
            await ProcessMonthlyBalanceAccordingToNewTransferData(command, currentMonthlyBalance, businessUnitId, transfer, cancellationToken);

            _logger.LogInformation("Transfer with Id {TransferId} successfully updated", command.TransferId);
            return Result.Ok(transfer);
        }

        private async Task UpdateBusinessUnitBalanceIfNeeded(
          double newValue,
          Guid businessUnitId,
          double valueToCancel,
          CancellationToken cancellationToken)
        {
            _logger.LogInformation("Checking if Business Unit with Id {BusinessUnitId} needs to update its balance", businessUnitId);
            if (newValue != valueToCancel)
            {
                _logger.LogInformation("Updating balance of Business Unit with Id {BusinessUnitId}", businessUnitId);
                var businessUnit = await _businessUnitRepository.GetByIdAsync(businessUnitId, cancellationToken);
                businessUnit.AddBalance(newValue);
                businessUnit.AddBalance(-valueToCancel);
                _businessUnitRepository.Update(businessUnit);
                _logger.LogInformation("Business Unit with Id {BusinessUnitId} updated", businessUnitId);
            }
        }

        private async Task ProcessMonthlyBalanceAccordingToNewTransferData(UpdateTransferCommand command, MonthlyBalance currentMonthlyBalance, Guid businessUnitId, Transfer transfer, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Checking if Transfer with Id {TransferId} needs to go to another Monthly Balance", command.TransferId);
            var shouldGoToAnotherMonthlyBalance = command.SettlementDate.Month != transfer.SettlementDate.Month ||
                command.SettlementDate.Year != transfer.SettlementDate.Year;
            transfer.Update(command.RelatedTo, command.Description, command.Value, command.SettlementDate, command.TransferType);

            if (shouldGoToAnotherMonthlyBalance)
            {
                currentMonthlyBalance.DeleteTransferById(transfer.Id);
                _monthlyBalanceRepository.Update(currentMonthlyBalance);

                var (month, year) = (command.SettlementDate.Month, command.SettlementDate.Year);
                _logger.LogInformation("Verifying if there is an existing Monthly Balance for {Month}/{Year} reference date", month, year);
                var monthlyBalance = await _monthlyBalanceRepository.GetByMonthAndYearAsync(month, year, cancellationToken);

                if (monthlyBalance is not null)
                {
                    _logger.LogInformation("Adding new Transfer(s) to Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance.Id);
                    monthlyBalance.AddTransfer(transfer);
                    _monthlyBalanceRepository.Update(monthlyBalance);
                    _logger.LogInformation("Monthly Balance with Id {MonthlyBalanceId} updated", monthlyBalance.Id);
                }
                else
                {
                    _logger.LogInformation("Creating new Monthly Balance");
                    monthlyBalance = new MonthlyBalance(businessUnitId, month, year);

                    _logger.LogInformation("Adding new Transfer(s) to Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance.Id);
                    monthlyBalance.AddTransfer(transfer);
                    _monthlyBalanceRepository.Insert(monthlyBalance);
                    _logger.LogInformation("New Monthly Balance created with Id {MonthlyBalanceId}", monthlyBalance.Id);
                }
            }
            else
            {
                _logger.LogInformation("Updating existing Monthly Balance with Id {MonthlyBalanceId}", currentMonthlyBalance.Id);
                currentMonthlyBalance.UpdateTransfer(transfer);
                _monthlyBalanceRepository.Update(currentMonthlyBalance);
            }
        }
    }
}
