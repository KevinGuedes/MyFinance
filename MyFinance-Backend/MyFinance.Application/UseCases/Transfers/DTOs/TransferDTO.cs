using MyFinance.Application.Common.DTO;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.UseCases.Transfers.DTOs;

public sealed class TransferDTO : EntityDTO
{
    public required string RelatedTo { get; init; }
    public required string Description { get; init; }
    public required DateTime SettlementDate { get; init; }
    public required TransferType Type { get; init; }
    public required double Value { get; init; }
}
