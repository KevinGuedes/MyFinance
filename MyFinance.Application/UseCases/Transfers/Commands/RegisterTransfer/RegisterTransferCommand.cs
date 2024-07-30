using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.Transfer.Requests;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;

public sealed class RegisterTransferCommand(RegisterTransferRequest request)
    : IUserRequiredRequest, ICommand<TransferResponse>
{
    public Guid CurrentUserId { get; set; }
    public Guid ManagementUnitId { get; init; } = request.ManagementUnitId;
    public Guid AccountTagId { get; init; } = request.AccountTagId;
    public Guid CategoryId { get; init; } = request.CategoryId;
    public decimal Value { get; init; } = request.Value;
    public DateTime SettlementDate { get; init; } = request.SettlementDate;
    public string RelatedTo { get; init; } = request.RelatedTo;
    public string Description { get; init; } = request.Description;
    public TransferType Type { get; init; } = request.Type;
}