using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.RequestHandling;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;

internal sealed class UpdateTransferHandler : CommandHandler<UpdateTransferCommand, Transfer>
{
    private readonly ILogger<UpdateTransferHandler> _logger;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;
    private readonly IBusinessUnitRepository _businessUnitRepository;
    private readonly ITransferRepository _transferRepository;

    public UpdateTransferHandler(
        ILogger<UpdateTransferHandler> logger,
        IMonthlyBalanceRepository monthlyBalanceRepository,
        IBusinessUnitRepository businessUnitRepository,
        ITransferRepository transferRepository)
    {
        _logger = logger;
        _monthlyBalanceRepository = monthlyBalanceRepository;
        _businessUnitRepository = businessUnitRepository;
        _transferRepository = transferRepository;
    }

    public async override Task<Result<Transfer>> Handle(UpdateTransferCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving current Monthly Balance of Transfer with Id {TransferId}", command.TransferId);
        var transfer = await _transferRepository.GetByIdAsync(command.TransferId, cancellationToken);
        var currentMonthlyBalance = transfer!.MonthlyBalance;
        var businessUnit = currentMonthlyBalance.BusinessUnit;

        var shouldGoToAnotherMonthlyBalance =
            currentMonthlyBalance.ReferenceYear != command.SettlementDate.Year ||
            currentMonthlyBalance.ReferenceMonth != command.SettlementDate.Month;

        if (!shouldGoToAnotherMonthlyBalance)
        {
            _logger.LogInformation("Updating balance of Business Unit with Id {BusinessUnitId}", businessUnit.Id);
            businessUnit.UpdateBalanceWithTransferDeletion(transfer.Value, transfer.Type);
            currentMonthlyBalance.UpdateBalanceWithTransferDeletion(transfer.Value, transfer.Type);
            transfer.Update(
                command.Value,
                command.RelatedTo,
                command.Description,
                command.SettlementDate,
                command.TransferType,
                currentMonthlyBalance);

            currentMonthlyBalance.UpdateBalanceWithNewTransfer(transfer.Value, transfer.Type);
            businessUnit.UpdateBalanceWithNewTransfer(transfer.Value, transfer.Type);

            _monthlyBalanceRepository.Update(currentMonthlyBalance);
            _transferRepository.Update(transfer);

            return Result.Ok(transfer);
        }
        else
        {
            var existingMonthlyBalance = await _monthlyBalanceRepository.GetByReferenceDateAndBusinessUnitId(command.SettlementDate, businessUnit.Id, cancellationToken);
            if (existingMonthlyBalance is null)
            {
                var newMonthlyBalance = new MonthlyBalance(command.SettlementDate, businessUnit);
                businessUnit.UpdateBalanceWithTransferDeletion(transfer.Value, transfer.Type);
                currentMonthlyBalance.UpdateBalanceWithTransferDeletion(transfer.Value, transfer.Type);

                transfer.Update(
                   command.Value,
                   command.RelatedTo,
                   command.Description,
                   command.SettlementDate,
                   command.TransferType,
                   newMonthlyBalance);

                newMonthlyBalance.UpdateBalanceWithNewTransfer(transfer.Value, transfer.Type);
                businessUnit.UpdateBalanceWithNewTransfer(transfer.Value, transfer.Type);

                _monthlyBalanceRepository.Update(currentMonthlyBalance);
                _monthlyBalanceRepository.Insert(newMonthlyBalance);
                _transferRepository.Update(transfer);

                return Result.Ok(transfer);
            }
            else
            {
                businessUnit.UpdateBalanceWithTransferDeletion(transfer.Value, transfer.Type);
                currentMonthlyBalance.UpdateBalanceWithTransferDeletion(transfer.Value, transfer.Type);

                transfer.Update(
                   command.Value,
                   command.RelatedTo,
                   command.Description,
                   command.SettlementDate,
                   command.TransferType,
                   existingMonthlyBalance);

                existingMonthlyBalance.UpdateBalanceWithNewTransfer(transfer.Value, transfer.Type);
                businessUnit.UpdateBalanceWithNewTransfer(transfer.Value, transfer.Type);

                _monthlyBalanceRepository.Update(currentMonthlyBalance);
                _monthlyBalanceRepository.Update(existingMonthlyBalance);
                _transferRepository.Update(transfer);

                return Result.Ok(transfer);
            }
        }
    }
}
