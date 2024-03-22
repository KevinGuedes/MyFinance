using MyFinance.Domain.Abstractions;
using MyFinance.Domain.Common;
using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities;

public class MonthlyBalance : Entity, IUserOwnedEntity
{
    private MonthlyBalance()
    {
    }

    public MonthlyBalance(DateTime referenceDate, BusinessUnit businessUnit, Guid userId)
    {
        Income = 0;
        Outcome = 0;
        ReferenceMonth = referenceDate.Month;
        ReferenceYear = referenceDate.Year;
        BusinessUnit = businessUnit;
        BusinessUnitId = businessUnit.Id;
        Transfers = [];
        UserId = userId;
    }

    public int ReferenceYear { get; init; }
    public int ReferenceMonth { get; init; }
    public double Income { get; private set; }
    public double Outcome { get; private set; }
    public double Balance => Income - Outcome;
    public Guid BusinessUnitId { get; private set; }
    public BusinessUnit BusinessUnit { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public List<Transfer> Transfers { get; private set; } = [];

    public void RegisterValue(double transferValue, TransferType transferType)
    {
        SetUpdateOnToUtcNow();

        if (transferType == TransferType.Profit)
            Income += transferValue;
        else
            Outcome += transferValue;
    }

    public void CancelValue(double transferValue, TransferType transferType)
    {
        SetUpdateOnToUtcNow();

        if (transferType == TransferType.Profit)
            Income -= transferValue;
        else
            Outcome -= transferValue;
    }
}