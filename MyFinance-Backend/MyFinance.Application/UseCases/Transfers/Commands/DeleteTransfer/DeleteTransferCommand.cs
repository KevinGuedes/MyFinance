using MyFinance.Application.Common.RequestHandling;

namespace MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;

public sealed class DeleteTransferCommand : Command
{
    public Guid TransferId { get; set; }

    public DeleteTransferCommand(Guid transferId)
        => TransferId = transferId;
}
