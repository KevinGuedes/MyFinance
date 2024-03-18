using MyFinance.Domain.Enums;

namespace MyFinance.Contracts.Transfer.Responses;

public sealed class TransferResponse
{
    public required Guid Id { get; init; }
    public required string RelatedTo { get; init; }
    public required string Description { get; init; }
    public required DateTime SettlementDate { get; init; }
    public required TransferType Type { get; init; }
    public required double Value { get; init; }
}
