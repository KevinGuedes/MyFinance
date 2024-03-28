using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;

public sealed record UpdateTransferCommand(
    Guid Id,
    Guid AccountTagId,
    Guid CategoryId,
    decimal Value,
    string RelatedTo,
    string Description,
    DateTime SettlementDate,
    TransferType Type)
    : IUserRequiredRequest, ICommand<TransferResponse>
{
    public Guid CurrentUserId { get; set; }
}