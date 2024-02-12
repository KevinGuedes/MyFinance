namespace MyFinance.Domain.Entities;

public class User : Entity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public List<BusinessUnit> BusinessUnits { get; set; } = [];
    public List<MonthlyBalance> MonthlyBalances { get; set; } = [];
    public List<Transfer> Transfers { get; set; } = [];
    public List<AccountTag> AccountTags { get; set; } = [];

    private User() { }

    public User(string name, string email, string passwordHash)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
    }
}
