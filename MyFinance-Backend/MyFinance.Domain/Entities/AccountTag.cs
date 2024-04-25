using MyFinance.Domain.Abstractions;
using MyFinance.Domain.Common;

namespace MyFinance.Domain.Entities;

public sealed class AccountTag : Entity, IUserOwnedEntity, IArchivableEntity
{
    private AccountTag()
    {
    }

    public AccountTag(string tag, string? description, Guid userId)
    {
        UserId = userId;
        Tag = tag;
        Description = description;
        IsArchived = false;
        ReasonToArchive = null;
        ArchivedOnUtc = null;
        Transfers = [];
    }

    public Guid UserId { get; init; }
    public string Tag { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsArchived { get; private set; }
    public string? ReasonToArchive { get; private set; }
    public DateTime? ArchivedOnUtc { get; private set; }
    public List<Transfer> Transfers { get; private set; } = [];

    public void Update(string tag, string? description)
    {
        SetUpdateOnToUtcNow();

        Tag = tag;
        Description = description;
    }

    public void Archive(string? reasonToArchive)
    {
        var utcNow = DateTime.UtcNow;
        SetUpdatedOnTo(utcNow);

        ArchivedOnUtc = DateTime.UtcNow;
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
}