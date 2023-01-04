using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Generics.Requests;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Domain.ValueObjects;

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
            _logger.LogInformation("Retrieving current Monthly Balance with Id {CurrentMonthlyBalanceId}", command.CurrentMonthlyBalanceId);
            var currentMonthlyBalance = await _monthlyBalanceRepository.GetByIdAsync(command.CurrentMonthlyBalanceId, cancellationToken);
            var transfer = currentMonthlyBalance.PopTransferById(command.TransferId);

            await Task.WhenAll(
                UpdateBusinessUnitBalance(currentMonthlyBalance.ReferenceData.BusinessUnitId, command.Value, transfer.Value, cancellationToken),
                ProcessMonthlyBalanceAccordingToUpdateData(command, currentMonthlyBalance, transfer, cancellationToken)
            );

            _logger.LogInformation("Transfer with Id {TransferId} successfully updated", command.TransferId);
            return Result.Ok(transfer);
        }

        private async Task UpdateBusinessUnitBalance(
            Guid businessUnitId,
            double transferNewValue,
            double transferCurrentValue,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Checking if Business Unit with Id {BusinessUnitId} needs to update its balance", businessUnitId);

            var shouldUpdateBusinessUnitBalance = transferCurrentValue != transferNewValue;
            if (shouldUpdateBusinessUnitBalance)
            {
                _logger.LogInformation("Updating balance of Business Unit with Id {BusinessUnitId}", businessUnitId);
                var businessUnit = await _businessUnitRepository.GetByIdAsync(businessUnitId, cancellationToken);
                businessUnit.AddBalance(transferNewValue - transferCurrentValue);
                _businessUnitRepository.Update(businessUnit);
                _logger.LogInformation("Business Unit with Id {BusinessUnitId} updated", businessUnitId);
            }
        }

        private async Task ProcessMonthlyBalanceAccordingToUpdateData(
            UpdateTransferCommand command, 
            MonthlyBalance currentMonthlyBalance, 
            Transfer transfer, 
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating Transfer with Id {TransferId}", transfer.Id);
            transfer.Update(command.RelatedTo, command.Description, command.Value, command.SettlementDate, command.TransferType);

            _logger.LogInformation("Checking if Transfer with Id {TransferId} needs to go to another Monthly Balance", transfer.Id);
            var currentReferenceData = currentMonthlyBalance.ReferenceData;
            var newReferenceData = new ReferenceData(currentMonthlyBalance.ReferenceData.BusinessUnitId, command.SettlementDate.Month, command.SettlementDate.Year);
            var shouldGoToAnotherMonthlyBalance = currentReferenceData != newReferenceData;
            
            _logger.LogInformation("Sending Transfer with Id {TransferId} to its corresponding Monthly Balance", transfer.Id);
            if (shouldGoToAnotherMonthlyBalance)
                await AddTransferToAnotherMonthlyBalance(newReferenceData, transfer, cancellationToken);
            else
            {
                _logger.LogInformation("Re-adding Transfer to existing Monthly Balance with Id {MonthlyBalanceId}", currentMonthlyBalance.Id);
                currentMonthlyBalance.AddTransfer(transfer);
            }

            _logger.LogInformation("Updating existing Monthly Balance with Id {MonthlyBalanceId}", currentMonthlyBalance.Id);
            _monthlyBalanceRepository.Update(currentMonthlyBalance);
        }

        private async Task AddTransferToAnotherMonthlyBalance(
            ReferenceData newReferenceData,
            Transfer transfer, 
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Verifying if there is an existing Monthly Balance for {ReferenceData}", newReferenceData);
            var monthlyBalance = await _monthlyBalanceRepository.GetByReferenceData(newReferenceData, cancellationToken);
            var shouldAddToExistingMonthlyBalance = monthlyBalance is null;

            if (shouldAddToExistingMonthlyBalance)
            {
                _logger.LogInformation("Creating new Monthly Balance");
                monthlyBalance = new MonthlyBalance(newReferenceData);

                _logger.LogInformation("Adding Transfer to Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance.Id);
                monthlyBalance.AddTransfer(transfer);
                _monthlyBalanceRepository.Insert(monthlyBalance);
                _logger.LogInformation("New Monthly Balance created with Id {MonthlyBalanceId}", monthlyBalance.Id);
            }
            else
            {
                _logger.LogInformation("Adding Transfer to Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance!.Id);
                monthlyBalance.AddTransfer(transfer);
                _monthlyBalanceRepository.Update(monthlyBalance);
                _logger.LogInformation("Monthly Balance with Id {MonthlyBalanceId} updated", monthlyBalance.Id);
            }
        }
    }
}
