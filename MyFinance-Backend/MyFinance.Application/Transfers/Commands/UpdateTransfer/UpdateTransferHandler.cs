﻿using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Generics.Requests;
using MyFinance.Application.Services.TransferProcessing;
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
            _logger.LogInformation("Retrieving current Monthly Balance with Id {CurrentMonthlyBalanceId}", command.CurrentMonthlyBalanceId);
            var currentMonthlyBalance = await _monthlyBalanceRepository.GetByIdAsync(command.CurrentMonthlyBalanceId, cancellationToken);
            var transfer = currentMonthlyBalance.PopTransferById(command.TransferId);

            await Task.WhenAll(
                UpdateBusinessUnitBalanceIfNeeded(currentMonthlyBalance.BusinessUnitId, command.Value, transfer.Value, cancellationToken),
                ProcessMonthlyBalanceAccordingToUpdateData(command, currentMonthlyBalance, transfer, cancellationToken)
            );

            _logger.LogInformation("Transfer with Id {TransferId} successfully updated", command.TransferId);
            return Result.Ok(transfer);
        }

        private async Task UpdateBusinessUnitBalanceIfNeeded(
            Guid businessUnitId,
            double newValue,
            double currentValue,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Checking if Business Unit with Id {BusinessUnitId} needs to update its balance", businessUnitId);
            if (TransferProcessingHelper.ShouldUpdateBusinessUnitBalance(currentValue, newValue))
            {
                _logger.LogInformation("Updating balance of Business Unit with Id {BusinessUnitId}", businessUnitId);
                var businessUnit = await _businessUnitRepository.GetByIdAsync(businessUnitId, cancellationToken);
                businessUnit.AddBalance(newValue - currentValue);
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
            _logger.LogInformation("Checking if Transfer with Id {TransferId} needs to go to another Monthly Balance", transfer.Id);
            var (month, year) = (command.SettlementDate.Month, command.SettlementDate.Year);
            var shouldGoToAnotherMonthlyBalance = TransferProcessingHelper.ShouldGoToAnotherMonthlyBalance(transfer.SettlementDate, command.SettlementDate);
            
            _logger.LogInformation("Updating Transfer with Id {TransferId}", transfer.Id);
            transfer.Update(command.RelatedTo, command.Description, command.Value, command.SettlementDate, command.TransferType);

            _logger.LogInformation("Sending Transfer with Id {TransferId} to its corresponding Monthly Balance", transfer.Id);
            if (shouldGoToAnotherMonthlyBalance)
                await AddTransferToAnotherMonthlyBalance(currentMonthlyBalance.BusinessUnitId, transfer, month, year, cancellationToken);
            else
            {
                _logger.LogInformation("Re-adding Transfer to existing Monthly Balance with Id {MonthlyBalanceId}", currentMonthlyBalance.Id);
                currentMonthlyBalance.AddTransfer(transfer);
            }

            _logger.LogInformation("Updating existing Monthly Balance with Id {MonthlyBalanceId}", currentMonthlyBalance.Id);
            _monthlyBalanceRepository.Update(currentMonthlyBalance);
        }

        private async Task AddTransferToAnotherMonthlyBalance(
            Guid businessUnitId, 
            Transfer transfer, 
            int month, 
            int year, 
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Verifying if there is an existing Monthly Balance for {Month}/{Year} reference date", month, year);
            var monthlyBalance = await _monthlyBalanceRepository.GetByMonthAndYearAsync(month, year, cancellationToken);

            if (monthlyBalance is not null)
            {
                _logger.LogInformation("Adding Transfer to Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance.Id);
                monthlyBalance.AddTransfer(transfer);
                _monthlyBalanceRepository.Update(monthlyBalance);
                _logger.LogInformation("Monthly Balance with Id {MonthlyBalanceId} updated", monthlyBalance.Id);
            }
            else
            {
                _logger.LogInformation("Creating new Monthly Balance");
                monthlyBalance = new MonthlyBalance(businessUnitId, month, year);

                _logger.LogInformation("Adding Transfer to Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance.Id);
                monthlyBalance.AddTransfer(transfer);
                _monthlyBalanceRepository.Insert(monthlyBalance);
                _logger.LogInformation("New Monthly Balance created with Id {MonthlyBalanceId}", monthlyBalance.Id);
            }
        }
    }
}
