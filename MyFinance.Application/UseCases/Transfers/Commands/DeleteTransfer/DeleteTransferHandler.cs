using FluentResults;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;

internal sealed class DeleteTransferHandler(IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<DeleteTransferCommand>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result> Handle(DeleteTransferCommand command, CancellationToken cancellationToken)
    {
        var transfer = await _myFinanceDbContext.Transfers
            .Include(transfer => transfer.ManagementUnit)
            .FirstOrDefaultAsync(transfer => transfer.Id == command.Id, cancellationToken);

        if (transfer is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Transfer with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        var managementUnit = transfer.ManagementUnit;
        managementUnit.CancelTransferValue(transfer.Value, transfer.Type);

        _myFinanceDbContext.ManagementUnits.Update(managementUnit);
        _myFinanceDbContext.Transfers.Remove(transfer);

        return Result.Ok();
    }
}