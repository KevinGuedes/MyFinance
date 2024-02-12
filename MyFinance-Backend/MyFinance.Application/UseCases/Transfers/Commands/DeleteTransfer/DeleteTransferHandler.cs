using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Application.Services.CurrentUserProvider;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;

internal sealed class DeleteTransferHandler(
    ILogger<DeleteTransferHandler> logger,
    IMonthlyBalanceRepository monthlyBalanceRepository,
    IBusinessUnitRepository businessUnitRepository,
    ITransferRepository transferRepository,
    ICurrentUserProvider currentUserProvider) : ICommandHandler<DeleteTransferCommand>
{
    private readonly ILogger<DeleteTransferHandler> _logger = logger;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository = monthlyBalanceRepository;
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ITransferRepository _transferRepository = transferRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<Result> Handle(DeleteTransferCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        _logger.LogInformation("Retrieving data required to delete Transfer with Id {TransferId}", command.Id);
        var transfer = await _transferRepository.GetByIdAsync(command.Id, currentUserId, cancellationToken);

        if (transfer is null)
        {
            _logger.LogWarning("Transfer with Id {BusinessUnitId} not found", command.Id);
            var errorMessage = string.Format("Transfer with Id {0} not found", command.Id);
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var monthlyBalance = transfer.MonthlyBalance;
        var businessUnit = monthlyBalance.BusinessUnit;

        _logger.LogInformation("Updating Monthly Balance with Id {MonthlyBalanceId}", monthlyBalance.Id);
        monthlyBalance.CancelValue(transfer.Value, transfer.Type);
        _monthlyBalanceRepository.Update(monthlyBalance);

        _logger.LogInformation("Updating Balance of Business Unit with Id {BusinessUnitId}", businessUnit.Id);
        businessUnit.CancelValue(transfer.Value, transfer.Type);
        _businessUnitRepository.Update(businessUnit);

        _logger.LogInformation("Deleting Transfer with Id {TransferId}", command.Id);
        _transferRepository.Delete(transfer);

        _logger.LogInformation("Transfer with Id {TransferId} sucessfully deleted", transfer.Id);
        return Result.Ok();
    }
}
