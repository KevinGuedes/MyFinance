using MyFinance.Application.Common.DTO;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.UseCases.Transfers.DTOs;

public sealed class TransferDTO : BaseDTO
{
    public string RelatedTo { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime SettlementDate { get; set; }
    public TransferType Type { get; set; }
    public double Value { get; set; }
}
