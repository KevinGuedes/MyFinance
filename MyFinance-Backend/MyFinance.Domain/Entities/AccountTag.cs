namespace MyFinance.Domain.Entities;

public class AccountTag : Entity
{
    public string Tag { get; private set; }
    public string? Description { get; private set; }
    public List<Transfer> Transfers { get; private set; }
    public List<BusinessUnit> BusinessUnits { get; private set; }

    protected AccountTag() { }

    public AccountTag(string tag, string? description)
    {
        Tag = tag;
        Description = description;
        Transfers = new List<Transfer>();
        BusinessUnits = new List<BusinessUnit>();
    }

    public void Update(string tag, string? description)
    {
        SetUpdateDateToNow();
        Tag = tag;
        Description = description;
    }
}
