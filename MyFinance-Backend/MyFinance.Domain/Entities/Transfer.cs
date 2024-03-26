using MyFinance.Domain.Abstractions;
using MyFinance.Domain.Common;
using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities;

public class Transfer : Entity, IUserOwnedEntity
{
    private Transfer()
    {
    }

    public Transfer(
        decimal value,
        string relatedTo,
        string description,
        DateTime settlementDate,
        TransferType type,
        BusinessUnit businessUnit,
        AccountTag accountTag,
        Guid userId)
    {
        Value = value;
        RelatedTo = relatedTo;
        Description = description;
        SettlementDate = settlementDate;
        Type = type;
        BusinessUnit = businessUnit;
        BusinessUnitId = businessUnit.Id;
        AccountTag = accountTag;
        AccountTagId = accountTag.Id;
        UserId = userId;
    }

    public decimal Value { get; private set; }
    public string RelatedTo { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime SettlementDate { get; private set; }
    public TransferType Type { get; private set; }
    public Guid UserId { get; private set; }
    public Guid BusinessUnitId { get; private set; }
    public BusinessUnit BusinessUnit { get; private set; } = null!;
    public Guid AccountTagId { get; private set; }
    public AccountTag AccountTag { get; private set; } = null!;

    public void Update(
        decimal value,
        string relatedTo,
        string description,
        DateTime settlementDate,
        TransferType type,
        AccountTag accountTag)
    {
        SetUpdateOnToUtcNow();

        Value = value;
        RelatedTo = relatedTo;
        Description = description;
        SettlementDate = settlementDate;
        Type = type;
        AccountTag = accountTag;
        AccountTagId = accountTag.Id;
    }

    public void Update(
        decimal value,
        string relatedTo,
        string description,
        DateTime settlementDate,
        TransferType type)
    {
        SetUpdateOnToUtcNow();

        Value = value;
        RelatedTo = relatedTo;
        Description = description;
        SettlementDate = settlementDate;
        Type = type;
    }
}