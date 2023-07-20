using MyFinance.Application.Common.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;

public sealed class DeleteTransferCommand : ICommand
{
    public Guid Id { get; set; }

    public DeleteTransferCommand(Guid id)
        => Id = id;
}
