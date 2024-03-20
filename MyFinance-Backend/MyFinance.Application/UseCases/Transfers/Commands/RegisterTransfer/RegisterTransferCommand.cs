using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.RequestHandling;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;

public sealed record RegisterTransferCommand(
    Guid BusinessUnitId,
    Guid AccountTagId,
    double Value,
    string RelatedTo,
    string Description,
    DateTime SettlementDate,
    TransferType Type)
    : UserBasedRequest, ICommand<TransferResponse>
{
}