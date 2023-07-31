using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities;

public class MonthlyBalance : Entity
{
    public int ReferenceYear { get; init; }
    public int ReferenceMonth { get; init; }
    public double Income { get; private set; }
    public double Outcome { get; private set; }
    public double Balance { get => Income - Outcome; }
    public Guid BusinessUnitId { get; private set; }
    public BusinessUnit BusinessUnit { get; private set; }
    public List<Transfer> Transfers { get; private set; }

    protected MonthlyBalance() { }

    public MonthlyBalance(DateTime referenceDate, BusinessUnit businessUnit)
    {
        Income = 0;
        Outcome = 0;
        ReferenceMonth = referenceDate.Month;
        ReferenceYear = referenceDate.Year;
        BusinessUnit = businessUnit;
        BusinessUnitId = businessUnit.Id;
        Transfers = new List<Transfer>();
    }

    public void RegisterValue(double transferValue, TransferType transferType)
    {
        SetUpdateDateToNow();
        if (transferType == TransferType.Profit) Income += transferValue;
        else Outcome += transferValue;
    }

    public void CancelValue(double transferValue, TransferType transferType)
    {
        SetUpdateDateToNow();
        if (transferType == TransferType.Profit) Income -= transferValue;
        else Outcome -= transferValue;
    }
}
