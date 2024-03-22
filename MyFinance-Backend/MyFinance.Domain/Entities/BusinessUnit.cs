using MyFinance.Domain.Abstractions;
using MyFinance.Domain.Common;
using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities;

public class BusinessUnit : Entity, IUserOwnedEntity, IArchivableEntity
{
    private BusinessUnit()
    {
    }

    public BusinessUnit(string name, string? description, Guid userId)
    {
        Name = name;
        Income = 0;
        Outcome = 0;
        Description = description;
        IsArchived = false;
        ReasonToArchive = null;
        ArchivedOnUtc = null;
        UserId = userId;
        MonthlyBalances = [];
    }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public double Income { get; private set; }
    public double Outcome { get; private set; }
    public double Balance => Income - Outcome;
    public bool IsArchived { get; private set; }
    public string? ReasonToArchive { get; private set; }
    public DateTime? ArchivedOnUtc { get; private set; }
    public Guid UserId { get; private set; }
    public List<MonthlyBalance> MonthlyBalances { get; private set; } = [];

    public void Update(string name, string? description)
    {
        SetUpdateOnToUtcNow();

        Name = name;
        Description = description;
    }

    public void Archive(string? reasonToArchive)
    {
        var utcNow = DateTime.UtcNow;
        SetUpdateOnToUtcNow(utcNow);

        ArchivedOnUtc = utcNow;
        IsArchived = true;
        ReasonToArchive = reasonToArchive;
    }

    public void Unarchive()
    {
        SetUpdateOnToUtcNow();

        IsArchived = false;
        ArchivedOnUtc = null;
        ReasonToArchive = null;
    }

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