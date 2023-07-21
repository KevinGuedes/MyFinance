using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfers;

public sealed class RegisterTransfersCommand : ICommand
{
    public Guid BusinessUnitId { get; set; }
    public double Value { get; set; }
    public string RelatedTo { get; set; }
    public string Description { get; set; }
    public DateTime SettlementDate { get; set; }
    public TransferType Type { get; set; }

    public RegisterTransfersCommand(
        Guid businessUnitId,
        double value,
        string relatedTo,
        string description,
        DateTime settlementDate,
        TransferType type)
    {
        BusinessUnitId = businessUnitId;
        Value = value;
        RelatedTo = relatedTo;
        Description = description;
        SettlementDate = settlementDate;
        Type = type;
    }
}
