using MyFinance.Domain.Enums;

namespace MyFinance.Contracts.Transfer.Requests;

public sealed record RegisterTransferRequest(
    Guid ManagementUnitId,
    Guid AccountTagId,
    Guid CategoryId,
    decimal Value,
    string RelatedTo,
    string Description,
    DateTime SettlementDate,
    TransferType Type)
{
}