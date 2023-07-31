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
        var (transferId, newTransferValue, relatedTo, description, settlementDate, type) = command;

        _logger.LogInformation("Retrieving current Monthly Balance of Transfer with Id {TransferId}", transferId);
        var transfer = await _transferRepository.GetByIdAsync(transferId, cancellationToken);

        if (transfer is null)
        {
            _logger.LogWarning("Transfer with Id {TransferId} not found", transferId);
            var errorMessage = string.Format("Transfer with Id {0} not found", transferId);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var currentMonthlyBalance = transfer.MonthlyBalance;
        var businessUnit = currentMonthlyBalance.BusinessUnit;

        businessUnit.CancelValue(transfer.Value, transfer.Type);
        currentMonthlyBalance.CancelValue(transfer.Value, transfer.Type);

        var shouldGoToAnotherMonthlyBalance =
            currentMonthlyBalance.ReferenceYear != settlementDate.Year ||
            currentMonthlyBalance.ReferenceMonth != settlementDate.Month;

        if (shouldGoToAnotherMonthlyBalance)
        {
            _logger.LogInformation("Adding Transfer with Id {TransferId} to another Monthly Balance", transfer.Id);
            currentMonthlyBalance.Transfers.Remove(transfer);

            _logger.LogInformation("Checking if there is a axisting Monthly Balance");
            var existingMonthlyBalance = await _monthlyBalanceRepository.GetByReferenceDateAndBusinessUnitId(
                settlementDate,
                businessUnit.Id,
                cancellationToken);

            if (existingMonthlyBalance is null)
            {
                var newMonthlyBalance = new MonthlyBalance(settlementDate, businessUnit);
                transfer.Update(newTransferValue, relatedTo, description, settlementDate, type, newMonthlyBalance);
                newMonthlyBalance.RegisterValue(transfer.Value, transfer.Type);
                _monthlyBalanceRepository.Insert(newMonthlyBalance);

                _logger.LogInformation(
                   "Transfer with Id {TransferId} added to new Monthly Balance with Id {MonthlyBalanceId}",
                   transfer.Id, newMonthlyBalance.Id);
            }
            else
            {
                transfer.Update(newTransferValue, relatedTo, description, settlementDate, type, existingMonthlyBalance);
                existingMonthlyBalance.RegisterValue(transfer.Value, transfer.Type);
                _monthlyBalanceRepository.Update(existingMonthlyBalance);

                _logger.LogInformation(
                   "Transfer with Id {TransferId} added to existing Monthly Balance with Id {MonthlyBalanceId}",
                   transfer.Id, existingMonthlyBalance.Id);
            }
        }
        else
        {
            transfer.Update(newTransferValue, relatedTo, description, settlementDate, type, currentMonthlyBalance);
            currentMonthlyBalance.RegisterValue(transfer.Value, transfer.Type);

            _logger.LogInformation(
                "Transfer with Id {TransferId} kept on the same Monthly Balance with Id {MonthlyBalanceId}",
                transfer.Id, currentMonthlyBalance.Id);
        }

        businessUnit.RegisterValue(transfer.Value, transfer.Type);
        _transferRepository.Update(transfer);
        _businessUnitRepository.Update(businessUnit);
        _monthlyBalanceRepository.Update(currentMonthlyBalance);
        _logger.LogInformation("Transfer with Id {TransferId} successfully updated", transfer.Id);

        return Result.Ok(transfer);
    }
}
