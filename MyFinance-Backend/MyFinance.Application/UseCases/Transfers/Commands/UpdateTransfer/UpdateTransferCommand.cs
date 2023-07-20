using MyFinance.Application.Common.RequestHandling;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;

public sealed class UpdateTransferCommand : Command<Transfer>
{
    public Guid TransferId { get; set; }
    public double Value { get; set; }
    public string RelatedTo { get; set; }
    public string Description { get; set; }
    public DateTime SettlementDate { get; set; }
    public TransferType TransferType { get; set; }

    public UpdateTransferCommand(
        Guid transferId,
        double value,
        string relatedTo,
        string description,
        DateTime settlementDate,
        TransferType transferType)
    {
        TransferId = transferId;
        Value = value;
        RelatedTo = relatedTo;
        Description = description;
        SettlementDate = settlementDate;
        TransferType = transferType;
    }
}
