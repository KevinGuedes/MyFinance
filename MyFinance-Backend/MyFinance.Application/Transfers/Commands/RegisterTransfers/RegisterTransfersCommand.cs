﻿using MyFinance.Application.Common.RequestHandling;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.Transfers.Commands.RegisterTransfers;

public sealed class RegisterTransfersCommand : Command
{
    public Guid BusinessUnitId { get; set; }
    public double Value { get; set; }
    public string RelatedTo { get; set; }
    public string Description { get; set; }
    public DateTime SettlementDate { get; set; }
    public TransferType TransferType { get; set; }

    public RegisterTransfersCommand(
        Guid businessUnitId,
        double value,
        string relatedTo,
        string description,
        DateTime settlementDate,
        TransferType transferType)
    {
        BusinessUnitId = businessUnitId;
        Value = value;
        RelatedTo = relatedTo;
        Description = description;
        SettlementDate = settlementDate;
        TransferType = transferType;
    }
}
