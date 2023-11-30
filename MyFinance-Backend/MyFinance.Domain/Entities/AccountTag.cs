namespace MyFinance.Domain.Entities;

public class AccountTag : Entity
{
    public string Tag { get; private set; }
    public string? Description { get; private set; }
    public bool IsArchived { get; private set; }
    public string? ReasonToArchive { get; private set; }
    public DateTime? ArchiveDate { get; private set; }
    public List<Transfer> Transfers { get; private set; }

    protected AccountTag() { }

    public AccountTag(string tag, string? description)
    {
        Tag = tag;
        Description = description;
        IsArchived = false;
        ReasonToArchive = null;
        ArchiveDate = null;
        Transfers = [];
    }

    public void Update(string tag, string? description)
    {
        SetUpdateDateToNow();
        Tag = tag;
        Description = description;
    }

    public void Archive(string reasonToArchive)
    {
        SetUpdateDateToNow();
        ArchiveDate = DateTime.UtcNow;
        IsArchived = true;
        ReasonToArchive = reasonToArchive;
    }

    public void Unarchive()
    {
        SetUpdateDateToNow();
        IsArchived = false;
        ArchiveDate = null;
        ReasonToArchive = null;
    }
}
