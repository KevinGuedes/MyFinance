using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities;

public class MonthlyBalance : Entity
{
    public double Income { get; private set; }
    public double Outcome { get; private set; }
    public int ReferenceMonth { get; init; }
    public int ReferenceYear { get; init; }
    public Guid BusinessUnitId { get; private set; }
    public BusinessUnit BusinessUnit { get; private set; }
    public IReadOnlyCollection<Transfer> Transfers { get; private set; }

    protected MonthlyBalance() { }

    public MonthlyBalance(DateTime referenceDate, BusinessUnit businessUnit)
    {
        Income = 0;
        Outcome = 0;
        ReferenceMonth = referenceDate.Month;
        ReferenceYear = referenceDate.Year;
        BusinessUnit = businessUnit;
        BusinessUnitId = businessUnit.Id;
        Transfers = new List<Transfer>().AsReadOnly();
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
