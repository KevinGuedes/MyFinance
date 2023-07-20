using MyFinance.Application.Common.RequestHandling;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;

public sealed class UpdateTransferCommand : Command<Transfer>
{
    public Guid Id { get; set; }
    public double Value { get; set; }
    public string RelatedTo { get; set; }
    public string Description { get; set; }
    public DateTime SettlementDate { get; set; }
    public TransferType Type { get; set; }

    public UpdateTransferCommand(
        Guid id,
        double value,
        string relatedTo,
        string description,
        DateTime settlementDate,
        TransferType type)
    {
        Id = id;
        Value = value;
        RelatedTo = relatedTo;
        Description = description;
        SettlementDate = settlementDate;
        Type = type;
    }
}
