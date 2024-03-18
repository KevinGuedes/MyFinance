using FluentResults;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;

internal sealed class DeleteTransferHandler(
    IMonthlyBalanceRepository monthlyBalanceRepository,
    IBusinessUnitRepository businessUnitRepository,
    ITransferRepository transferRepository,
    ICurrentUserProvider currentUserProvider) : ICommandHandler<DeleteTransferCommand>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository = monthlyBalanceRepository;
    private readonly ITransferRepository _transferRepository = transferRepository;

    public async Task<Result> Handle(DeleteTransferCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.GetCurrentUserId();

        var transfer = await _transferRepository.GetByIdAsync(command.Id, currentUserId, cancellationToken);

        if (transfer is null)
        {
            var errorMessage = $"Transfer with Id {command.Id} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var monthlyBalance = transfer.MonthlyBalance;
        var businessUnit = monthlyBalance.BusinessUnit;

        monthlyBalance.CancelValue(transfer.Value, transfer.Type);
        _monthlyBalanceRepository.Update(monthlyBalance);

        businessUnit.CancelValue(transfer.Value, transfer.Type);
        _businessUnitRepository.Update(businessUnit);

        _transferRepository.Delete(transfer);

        return Result.Ok();
    }
}