using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;

internal sealed class DeleteTransferHandler(
    IManagementUnitRepository managementUnitRepository,
    ITransferRepository transferRepository) : ICommandHandler<DeleteTransferCommand>
{
    private readonly IManagementUnitRepository _managementUnitRepository = managementUnitRepository;
    private readonly ITransferRepository _transferRepository = transferRepository;

    public async Task<Result> Handle(DeleteTransferCommand command, CancellationToken cancellationToken)
    {
        var transfer = await _transferRepository.GetWithManagementUnitByIdAsync(command.Id, cancellationToken);

        if (transfer is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Transfer with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        var managementUnit = transfer.ManagementUnit;
        managementUnit.CancelTransferValue(transfer.Value, transfer.Type);
        _managementUnitRepository.Update(managementUnit);
        _transferRepository.Delete(transfer);

        return Result.Ok();
    }
}