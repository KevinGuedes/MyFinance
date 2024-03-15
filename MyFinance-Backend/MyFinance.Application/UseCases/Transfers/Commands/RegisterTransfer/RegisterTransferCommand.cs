using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;

public sealed record RegisterTransferCommand(
    Guid BusinessUnitId,
    Guid AccountTagId,
    double Value,
    string RelatedTo,
    string Description,
    DateTime SettlementDate,
    TransferType Type) : ICommand<Transfer>
{
}