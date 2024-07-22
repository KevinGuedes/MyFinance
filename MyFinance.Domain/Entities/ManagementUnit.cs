using MyFinance.Domain.Abstractions;
using MyFinance.Domain.Common;
using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities;

public sealed class ManagementUnit : Entity, IUserOwnedEntity, IArchivableEntity
{
    private ManagementUnit()
    {
    }

    public ManagementUnit(string name, string? description, Guid userId)
    {
        Name = name;
        Income = 0;
        Outcome = 0;
        Description = description;
        IsArchived = false;
        ReasonToArchive = null;
        ArchivedOnUtc = null;
        UserId = userId;
        Transfers = [];
    }

    public Guid UserId { get; init; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public decimal Income { get; private set; }
    public decimal Outcome { get; private set; }
    public decimal Balance => Income - Outcome;
    public bool IsArchived { get; private set; }
    public string? ReasonToArchive { get; private set; }
    public DateTime? ArchivedOnUtc { get; private set; }
    public List<Transfer> Transfers { get; private set; } = [];
    public List<AccountTag> AccountTags { get; private set; } = [];
    public List<Category> Categories { get; private set; } = [];

    public void Update(string name, string? description)
    {
        SetUpdateOnToUtcNow();

        Name = name;
        Description = description;
    }

    public void Archive(string? reasonToArchive)
    {
        var utcNow = DateTime.UtcNow;
        SetUpdatedOnTo(utcNow);

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

    public void RegisterTransferValue(decimal transferValue, TransferType transferType)
    {
        SetUpdateOnToUtcNow();

        if (transferType == TransferType.Profit)
            Income += transferValue;
        else
            Outcome += transferValue;
    }

    public void CancelTransferValue(decimal transferValue, TransferType transferType)
    {
        SetUpdateOnToUtcNow();

        if (transferType == TransferType.Profit)
            Income -= transferValue;
        else
            Outcome -= transferValue;
    }
}