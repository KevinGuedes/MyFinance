using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;

internal sealed class UpdateTransferHandler : ICommandHandler<UpdateTransferCommand, Transfer>
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

    public async Task<Result<Transfer>> Handle(UpdateTransferCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving current Monthly Balance of Transfer with Id {TransferId}", command.Id);
        var transfer = await _transferRepository.GetByIdAsync(command.Id, cancellationToken);

        if (transfer is null)
        {
            _logger.LogWarning("Transfer with Id {BusinessUnitId} not found", command.Id);
            var errorMessage = string.Format("Transfer with Id {0} not found", command.Id);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var currentMonthlyBalance = transfer.MonthlyBalance;
        var businessUnit = currentMonthlyBalance.BusinessUnit;

        _logger.LogInformation(
            "Cancelling value from Bussines Unit with Id {BusinessUnitId}", businessUnit.Id);
        businessUnit.CancelValue(transfer.Value, transfer.Type);

        _logger.LogInformation(
            "Cancelling value from current Monthly Balance with Id {MonthlyBalanceId}", currentMonthlyBalance.Id);
        currentMonthlyBalance.CancelValue(transfer.Value, transfer.Type);

        var shouldGoToAnotherMonthlyBalance =
            currentMonthlyBalance.ReferenceYear != command.SettlementDate.Year ||
            currentMonthlyBalance.ReferenceMonth != command.SettlementDate.Month;

        if (shouldGoToAnotherMonthlyBalance)
        {
            _logger.LogInformation("Adding Transfer with Id {TransferId} to another Monthly Balance", transfer.Id);
            currentMonthlyBalance.Transfers.Remove(transfer);

            _logger.LogInformation("Checking if there is a axisting Monthly Balance");
            var existingMonthlyBalance = await _monthlyBalanceRepository.GetByReferenceDateAndBusinessUnitId(
                command.SettlementDate,
                businessUnit.Id,
                cancellationToken);

            if (existingMonthlyBalance is null)
            {
                var newMonthlyBalance = new MonthlyBalance(command.SettlementDate, businessUnit);

                transfer.Update(
                   command.Value,
                   command.RelatedTo,
                   command.Description,
                   command.SettlementDate,
                   command.Type,
                   newMonthlyBalance);

                _logger.LogInformation(
                    "Transfer with Id {TransferId} added to new Monthly Balance with Id {MonthlyBalanceId}",
                    transfer.Id, newMonthlyBalance.Id);

                _logger.LogInformation(
                    "Registering new value to new Monthly Balance with Id {MonthlyBalanceId}", newMonthlyBalance.Id);
                newMonthlyBalance.RegisterValue(transfer.Value, transfer.Type);

                _monthlyBalanceRepository.Insert(newMonthlyBalance);
                _logger.LogInformation("New Monthly Balance with Id {MonthlyBalanceId} inserted", newMonthlyBalance.Id);
            }
            else
            {
                transfer.Update(
                   command.Value,
                   command.RelatedTo,
                   command.Description,
                   command.SettlementDate,
                   command.Type,
                   existingMonthlyBalance);

                _logger.LogInformation(
                    "Transfer with Id {TransferId} added to existing Monthly Balance with Id {MonthlyBalanceId}",
                    transfer.Id, existingMonthlyBalance.Id);

                _logger.LogInformation(
                   "Registering new value to existing Monthly Balance with Id {MonthlyBalanceId}", existingMonthlyBalance.Id);
                existingMonthlyBalance.RegisterValue(transfer.Value, transfer.Type);

                _monthlyBalanceRepository.Update(existingMonthlyBalance);
                _logger.LogInformation("Existing Monthly Balance with Id {MonthlyBalanceId} successfully updated", existingMonthlyBalance.Id);
            }
        }
        else
        {
            transfer.Update(
                command.Value,
                command.RelatedTo,
                command.Description,
                command.SettlementDate,
                command.Type,
                currentMonthlyBalance);

            _logger.LogInformation(
                "Registering new value to current Monthly Balance with Id {MonthlyBalanceId}", currentMonthlyBalance.Id);
            currentMonthlyBalance.RegisterValue(transfer.Value, transfer.Type);
        }

        _logger.LogInformation("Registering new value to Bussines Unit with Id {BusinessUnitId}", businessUnit.Id);
        businessUnit.RegisterValue(transfer.Value, transfer.Type);

        _transferRepository.Update(transfer);
        _logger.LogInformation("Transfer with Id {TransferId} successfully updated", transfer.Id);

        _businessUnitRepository.Update(businessUnit);
        _logger.LogInformation("Business Unit with Id {BusinessUnitId} successfully updated", businessUnit.Id);

        _monthlyBalanceRepository.Update(currentMonthlyBalance);
        _logger.LogInformation("Current Monthly Balance with Id {MonthlyBalanceId} successfully updated", currentMonthlyBalance.Id);

        return Result.Ok(transfer);
    }
}
