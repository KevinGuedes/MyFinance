using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities;

public class Transfer : Entity
{
    public double Value { get; private set; }
    public string RelatedTo { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime SettlementDate { get; private set; }
    public TransferType Type { get; private set; }
    public Guid MonthlyBalanceId { get; private set; }
    public MonthlyBalance MonthlyBalance { get; private set; } = null!;
    public Guid AccountTagId { get; private set; }
    public AccountTag AccountTag { get; private set; } = null!;

    protected Transfer() { }

    public Transfer(
        double value,
        string relatedTo,
        string description,
        DateTime settlementDate,
        TransferType type,
        MonthlyBalance monthlyBalance,
        AccountTag accountTag)
    {
        Value = value;
        RelatedTo = relatedTo;
        Description = description;
        SettlementDate = settlementDate;
        Type = type;
        MonthlyBalance = monthlyBalance;
        MonthlyBalanceId = monthlyBalance.Id;
        AccountTag = accountTag;
        AccountTagId = accountTag.Id;
    }

    public void Update(
        double value,
        string relatedTo,
        string description,
        DateTime settlementDate,
        TransferType type,
        MonthlyBalance monthlyBalance,
        AccountTag accountTag)
    {
        SetUpdateDateToNow();
        Value = value;
        RelatedTo = relatedTo;
        Description = description;
        SettlementDate = settlementDate;
        Type = type;
        MonthlyBalance = monthlyBalance;
        MonthlyBalanceId = monthlyBalance.Id;
        AccountTag = accountTag;
        AccountTagId = accountTag.Id;
    }
}
