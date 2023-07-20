using MyFinance.Application.Common.DTO;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.UseCases.Transfers.DTOs;

public class TransferDTO : BaseDTO
{
    public string RelatedTo { get; set; }
    public string Description { get; set; }
    public DateTime SettlementDate { get; set; }
    public TransferType Type { get; set; }
    public double Value { get; set; }

    public TransferDTO(
        Guid id,
        string relatedTo,
        string description,
        DateTime settlementDate,
        TransferType type,
        double value)
    {
        Id = id;
        RelatedTo = relatedTo;
        Description = description;
        SettlementDate = settlementDate;
        Type = type;
        Value = value;
    }
}
