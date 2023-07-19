using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities;

public class MonthlyBalance : Entity
{
    public double Income { get; private set; }
    public double Outcome { get; private set; }
    public DateTime ReferenceDate { get; private set; }
    public int ReferenceMonth { get => ReferenceDate.Month; }
    public int ReferenceYear { get => ReferenceDate.Year; }
    public Guid BusinessUnitId { get; private set; }
    public BusinessUnit BusinessUnit { get; private set; }
    public List<Transfer> Transfers { get; private set; }
    
    protected MonthlyBalance() { }

    public MonthlyBalance(DateTime referenceDate, BusinessUnit businessUnit)
    {
        Transfers = new List<Transfer>();
        Income = 0;
        Outcome = 0;
        ReferenceDate = referenceDate;
        BusinessUnit = businessUnit;
        BusinessUnitId = businessUnit.Id;
    }

    public void UpdateBalanceWithNewTransfer(double transferValue, TransferType transferType)
    {
        if (transferType == TransferType.Profit) Income += transferValue;
        else Outcome += transferValue;

        SetUpdateDateToNow();
    }

    public void UpdateBalanceWithTransferDeletion(double transferValue, TransferType transferType)
    {
        if (transferType == TransferType.Profit) Income -= transferValue;
        else Outcome -= transferValue;

        SetUpdateDateToNow();
    }
}
