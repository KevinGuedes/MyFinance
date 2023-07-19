using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.RequestHandling;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Transfers.Commands.DeleteTransfer;

internal sealed class DeleteTransferHandler : CommandHandler<DeleteTransferCommand>
{
    private readonly ILogger<DeleteTransferHandler> _logger;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;
    private readonly IBusinessUnitRepository _businessUnitRepository;
    private readonly ITransferRepository _transferRepository;

    public DeleteTransferHandler(
        ILogger<DeleteTransferHandler> logger,
        IMonthlyBalanceRepository monthlyBalanceRepository,
        IBusinessUnitRepository businessUnitRepository,
        ITransferRepository transferRepository)
    {
        _logger = logger;
        _monthlyBalanceRepository = monthlyBalanceRepository;
        _businessUnitRepository = businessUnitRepository;
        _transferRepository = transferRepository;
    }

    public async override Task<Result> Handle(DeleteTransferCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving data required to delete Transfer with Id {TransferId}", command.TransferId);
        var monthlyBalance = await _monthlyBalanceRepository.GetByIdAsync(command.MonthlyBalanceId, cancellationToken);
        var getBusinessUnitTask = _businessUnitRepository.GetByIdAsync(monthlyBalance!.BusinessUnitId, cancellationToken);
        var getTransferTask = _transferRepository.GetByIdAsync(command.TransferId, cancellationToken);
        await Task.WhenAll(getBusinessUnitTask, getTransferTask);
        var businessUnit = await getBusinessUnitTask;
        var transfer = await getTransferTask;

        _logger.LogInformation("Deleting Transfer with Id {TransferId}", command.TransferId);
        _transferRepository.Delete(transfer!);   

        _logger.LogInformation("Updating Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance.Id);
        monthlyBalance.UpdateBalanceWithTransferDeletion(transfer!.Value, transfer!.Type);
        _monthlyBalanceRepository.Update(monthlyBalance);

        _logger.LogInformation("Updating Balance of Business Unit with Id {BusinessUnitId}", businessUnit!.Id);
        businessUnit.UpdateBalanceWithTransferDeletion(transfer!.Value, transfer!.Type);
        _businessUnitRepository.Update(businessUnit);

        _logger.LogInformation("Transfer with Id {TransferId} sucessfully deleted", transfer.Id);
        return Result.Ok();
    }
}
