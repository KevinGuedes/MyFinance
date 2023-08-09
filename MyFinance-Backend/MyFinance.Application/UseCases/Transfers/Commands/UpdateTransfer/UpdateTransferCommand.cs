using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;

public sealed record UpdateTransferCommand(
    Guid Id,
    Guid AccountTagId,
    double Value,
    string RelatedTo,
    string Description,
    DateTime SettlementDate,
    TransferType Type) : ICommand<Transfer>
{
}
