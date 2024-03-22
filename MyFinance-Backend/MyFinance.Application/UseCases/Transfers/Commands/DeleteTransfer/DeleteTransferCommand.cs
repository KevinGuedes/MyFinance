using MyFinance.Application.Abstractions.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;

public sealed record DeleteTransferCommand(Guid Id) : ICommand
{
}