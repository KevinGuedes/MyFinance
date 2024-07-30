using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.Transfer.Requests;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;

public sealed class UpdateTransferCommand(UpdateTransferRequest request) : ICommand<TransferResponse>
{
    public Guid Id { get; init; } = request.Id;
    public Guid AccountTagId { get; init; } = request.AccountTagId;
    public Guid CategoryId { get; init; } = request.CategoryId;
    public decimal Value { get; init; } = request.Value;
    public DateTime SettlementDate { get; init; } = request.SettlementDate;
    public string RelatedTo { get; init; } = request.RelatedTo;
    public string Description { get; init; } = request.Description;
    public TransferType Type { get; init; } = request.Type;
}