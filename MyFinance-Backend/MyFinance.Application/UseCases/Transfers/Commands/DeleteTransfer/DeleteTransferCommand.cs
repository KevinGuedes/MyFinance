using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.RequestHandling;

namespace MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;

public sealed record DeleteTransferCommand(Guid Id) : UserBasedRequest, ICommand
{
}
