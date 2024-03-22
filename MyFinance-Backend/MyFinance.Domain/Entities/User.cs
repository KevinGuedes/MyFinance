using MyFinance.Domain.Common;

namespace MyFinance.Domain.Entities;

public class User : Entity
{
    private User()
    {
    }

    public User(string name, string email, string passwordHash)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
    }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
}