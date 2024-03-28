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
        Guid userId,
        BusinessUnit businessUnit,
        AccountTag accountTag,
        Category category)
    {
        Value = value;
        RelatedTo = relatedTo;
        Description = description;
        SettlementDate = settlementDate;
        Type = type;
        UserId = userId;
        BusinessUnit = businessUnit;
        BusinessUnitId = businessUnit.Id;
        AccountTag = accountTag;
        AccountTagId = accountTag.Id;
        Category = category;
        CategoryId = category.Id;
    }

    public Guid UserId { get; init; }
    public decimal Value { get; private set; }
    public string RelatedTo { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime SettlementDate { get; private set; }
    public TransferType Type { get; private set; }
    public Guid BusinessUnitId { get; private set; }
    public BusinessUnit BusinessUnit { get; private set; } = null!;
    public Guid AccountTagId { get; private set; }
    public AccountTag AccountTag { get; private set; } = null!;
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    public void UpdateCategory(Category category)
    {
        SetUpdateOnToUtcNow();
       
        Category = category;
        CategoryId = category.Id;
    }

    public void UpdateAccountTag(AccountTag accountTag)
    {
        SetUpdateOnToUtcNow();

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