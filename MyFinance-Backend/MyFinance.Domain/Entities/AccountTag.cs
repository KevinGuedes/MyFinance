namespace MyFinance.Domain.Entities;

public class AccountTag : Entity
{
    public string Tag { get; private set; }
    public string? Description { get; private set; }

    public AccountTag(string tag, string? description)
        => (Tag, Description) = (tag, description);

    protected AccountTag() { }
}
