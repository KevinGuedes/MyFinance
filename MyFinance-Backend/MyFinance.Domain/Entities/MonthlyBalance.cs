using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities;

public class MonthlyBalance : UserOwnedEntity
{
    public int ReferenceYear { get; init; }
    public int ReferenceMonth { get; init; }
    public double Income { get; private set; }
    public double Outcome { get; private set; }
    public double Balance { get => Income - Outcome; }
    public Guid BusinessUnitId { get; private set; }
    public BusinessUnit BusinessUnit { get; private set; } = null!;
    public List<Transfer> Transfers { get; private set; } = [];

    private MonthlyBalance() { }

    public MonthlyBalance(DateTime referenceDate, BusinessUnit businessUnit, Guid userId) : base(userId)
    {
        Income = 0;
        Outcome = 0;
        ReferenceMonth = referenceDate.Month;
        ReferenceYear = referenceDate.Year;
        BusinessUnit = businessUnit;
        BusinessUnitId = businessUnit.Id;
        Transfers = [];
    }

    public void RegisterValue(double transferValue, TransferType transferType)
    {
        SetUpdateDateToNow();
        if (transferType == TransferType.Profit)
            Income += transferValue;
        else
            Outcome += transferValue;
    }

    public void CancelValue(double transferValue, TransferType transferType)
    {
        SetUpdateDateToNow();
        if (transferType == TransferType.Profit)
            Income -= transferValue;
        else
            Outcome -= transferValue;
    }
}
