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

        var shouldGoToAnotherMonthlyBalance =
            currentMonthlyBalance.ReferenceYear != command.SettlementDate.Year ||
            currentMonthlyBalance.ReferenceMonth != command.SettlementDate.Month;

        if (shouldGoToAnotherMonthlyBalance)
        {
            var existingMonthlyBalance = await _monthlyBalanceRepository.GetByReferenceDateAndBusinessUnitId(command.SettlementDate, businessUnit.Id, cancellationToken);
            if (existingMonthlyBalance is null)
            {
                var newMonthlyBalance = new MonthlyBalance(command.SettlementDate, businessUnit);
                businessUnit.CancelValue(transfer.Value, transfer.Type);
                currentMonthlyBalance.CancelValue(transfer.Value, transfer.Type);

                transfer.Update(
                   command.Value,
                   command.RelatedTo,
                   command.Description,
                   command.SettlementDate,
                   command.Type,
                   newMonthlyBalance);

                newMonthlyBalance.RegisterValue(transfer.Value, transfer.Type);
                businessUnit.RegisterValue(transfer.Value, transfer.Type);

                _transferRepository.Update(transfer);
                _businessUnitRepository.Update(businessUnit);
                _monthlyBalanceRepository.Update(currentMonthlyBalance);
                _monthlyBalanceRepository.Insert(newMonthlyBalance);

                return Result.Ok(transfer);
            }
            else
            {
                businessUnit.CancelValue(transfer.Value, transfer.Type);
                currentMonthlyBalance.CancelValue(transfer.Value, transfer.Type);

                transfer.Update(
                   command.Value,
                   command.RelatedTo,
                   command.Description,
                   command.SettlementDate,
                   command.Type,
                   existingMonthlyBalance);

                existingMonthlyBalance.RegisterValue(transfer.Value, transfer.Type);
                businessUnit.RegisterValue(transfer.Value, transfer.Type);

                _transferRepository.Update(transfer);
                _businessUnitRepository.Update(businessUnit);
                _monthlyBalanceRepository.Update(currentMonthlyBalance);
                _monthlyBalanceRepository.Update(existingMonthlyBalance);

                return Result.Ok(transfer);
            }
        }
        else
        {
            _logger.LogInformation("Updating balance of Business Unit with Id {BusinessUnitId}", businessUnit.Id);
            businessUnit.CancelValue(transfer.Value, transfer.Type);
            currentMonthlyBalance.CancelValue(transfer.Value, transfer.Type);
            transfer.Update(
                command.Value,
                command.RelatedTo,
                command.Description,
                command.SettlementDate,
                command.Type,
                currentMonthlyBalance);

            currentMonthlyBalance.RegisterValue(transfer.Value, transfer.Type);
            businessUnit.RegisterValue(transfer.Value, transfer.Type);

            _businessUnitRepository.Update(businessUnit);
            _monthlyBalanceRepository.Update(currentMonthlyBalance);
            _transferRepository.Update(transfer);

            return Result.Ok(transfer);
        }
    }
}
