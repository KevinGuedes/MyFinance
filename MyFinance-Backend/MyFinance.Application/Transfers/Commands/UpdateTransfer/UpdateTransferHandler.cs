using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;
using MyFinance.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Application.Transfers.Commands.UpdateTransfer
{
    internal sealed class UpdateTransferHandler : IRequestHandler<UpdateTransferCommand, Transfer>
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

        public async Task<Transfer> Handle(UpdateTransferCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving previous Monthly Balance with Id {PreviousMonthlyBalanceId}", command.CurrentMonthlyBalanceId);
            var currentMonthlyBalance = await _monthlyBalanceRepository.GetByIdAsync(command.CurrentMonthlyBalanceId, cancellationToken);
            var transfer = currentMonthlyBalance.GetTransferById(command.TransferId);

            var newFormattedValue = command.TransferType == TransferType.Profit ? command.AbsoluteValue : -command.AbsoluteValue;
            var shouldUpdateBusinessUnitBalance = transfer.FormattedValue != newFormattedValue;
            if (shouldUpdateBusinessUnitBalance)
            {
                var businessUnit = await _businessUnitRepository.GetByIdAsync(currentMonthlyBalance.BusinessUnitId, cancellationToken);
                businessUnit.AddBalance(-transfer.FormattedValue);
                _businessUnitRepository.Update(businessUnit);
            }

            var shouldGoToAnotherMonthlyBalance = command.SettlementDate.Month != transfer.SettlementDate.Month || 
                command.SettlementDate.Year != transfer.SettlementDate.Year;
            transfer.Update(command.RelatedTo, command.Description, command.AbsoluteValue, command.SettlementDate, command.TransferType);
            
            if (shouldGoToAnotherMonthlyBalance)
            {
                var (month, year) = (command.SettlementDate.Month, command.SettlementDate.Year);
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
                    monthlyBalance = new MonthlyBalance(currentMonthlyBalance.BusinessUnitId, month, year);

                    _logger.LogInformation("Adding new Transfer(s) to Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance.Id);
                    monthlyBalance.AddTransfer(transfer);
                    _monthlyBalanceRepository.Insert(monthlyBalance);
                    _logger.LogInformation("New Monthly Balance created with Id {MonthlyBalanceId}", monthlyBalance.Id);
                }
            }
            else
            {
                currentMonthlyBalance.UpdateTransfer(transfer);
                _monthlyBalanceRepository.Update(currentMonthlyBalance);
            }

            return transfer;
        }
    }
}
