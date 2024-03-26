﻿using MyFinance.Domain.Enums;

namespace MyFinance.Contracts.Transfer.Requests;

public sealed class RegisterTransferRequest
{
    public required Guid BusinessUnitId { get; init; }
    public required Guid AccountTagId { get; init; }
    public required decimal Value { get; init; }
    public required string RelatedTo { get; init; }
    public required string Description { get; init; }
    public required DateTime SettlementDate { get; init; }
    public required TransferType Type { get; init; }
}