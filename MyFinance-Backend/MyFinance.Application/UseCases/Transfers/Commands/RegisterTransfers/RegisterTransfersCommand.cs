using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfers;

public sealed record RegisterTransfersCommand(
    Guid BusinessUnitId,
    double Value,
    string RelatedTo,
    string Description,
    DateTime SettlementDate,
    TransferType Type) : ICommand<Transfer>
{ }
