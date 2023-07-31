using MyFinance.Application.Common.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;

public sealed record DeleteTransferCommand(Guid Id) : ICommand
{
}
