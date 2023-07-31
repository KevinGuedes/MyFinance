using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities;

public class Transfer : Entity
{
    public double Value { get; private set; }
    public string RelatedTo { get; private set; }
    public string Description { get; private set; }
    public DateTime SettlementDate { get; private set; }
    public TransferType Type { get; private set; }
    public Guid MonthlyBalanceId { get; private set; }
    public MonthlyBalance MonthlyBalance { get; private set; }

    protected Transfer() { }

    public Transfer(
        double value,
        string relatedTo,
        string description,
        DateTime settlementDate,
        TransferType type,
        MonthlyBalance monthlyBalance)
    {
        Value = value;
        RelatedTo = relatedTo;
        Description = description;
        SettlementDate = settlementDate;
        Type = type;
        MonthlyBalance = monthlyBalance;
        MonthlyBalanceId = monthlyBalance.Id;
    }

    public void Update(
        double value,
        string relatedTo,
        string description,
        DateTime settlementDate,
        TransferType type,
        MonthlyBalance monthlyBalance)
    {
        SetUpdateDateToNow();
        Value = value;
        RelatedTo = relatedTo;
        Description = description;
        SettlementDate = settlementDate;
        Type = type;
        MonthlyBalance = monthlyBalance;
        MonthlyBalanceId = monthlyBalance.Id;
    }
}
