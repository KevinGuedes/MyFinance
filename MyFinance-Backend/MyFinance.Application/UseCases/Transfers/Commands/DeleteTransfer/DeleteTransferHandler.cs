using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;

internal sealed class DeleteTransferHandler(
    IBusinessUnitRepository businessUnitRepository,
    ITransferRepository transferRepository) : ICommandHandler<DeleteTransferCommand>
{
    private readonly IBusinessUnitRepository _businessUnitRepository = businessUnitRepository;
    private readonly ITransferRepository _transferRepository = transferRepository;

    public async Task<Result> Handle(DeleteTransferCommand command, CancellationToken cancellationToken)
    {
        var transfer = await _transferRepository.GetWithBusinessUnitByIdAsync(command.Id, cancellationToken);

        if (transfer is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Transfer with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        var businessUnit = transfer.BusinessUnit;
        businessUnit.CancelTransferValue(transfer.Value, transfer.Type);
        _businessUnitRepository.Update(businessUnit);
        _transferRepository.Delete(transfer);

        return Result.Ok();
    }
}