using MyFinance.Domain.Abstractions;
using MyFinance.Domain.Common;
using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities;

public sealed class Transfer : Entity, IUserOwnedEntity
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
        Guid managementUnitId,
        Guid accountTagId,
        Guid categoryId,
        Guid userId)
    {
        Value = value;
        RelatedTo = relatedTo;
        Description = description;
        SettlementDate = settlementDate;
        Type = type;
        UserId = userId;
        ManagementUnitId = managementUnitId;
        AccountTagId = accountTagId;
        CategoryId = categoryId;
    }

    public Guid UserId { get; init; }
    public decimal Value { get; private set; }
    public string RelatedTo { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime SettlementDate { get; private set; }
    public TransferType Type { get; private set; }
    public Guid ManagementUnitId { get; private set; }
    public ManagementUnit ManagementUnit { get; private set; } = null!;
    public Guid AccountTagId { get; private set; }
    public AccountTag AccountTag { get; private set; } = null!;
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    public void UpdateCategory(Guid categoryId)
    {
        SetUpdateOnToUtcNow();
        CategoryId = categoryId;
    }

    public void UpdateAccountTag(Guid accountTagId)
    {
        SetUpdateOnToUtcNow();
        AccountTagId = accountTagId;
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