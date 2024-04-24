using MyFinance.Domain.Common;

namespace MyFinance.Domain.Entities;

public sealed class User : Entity
{
    private User()
    {
    }

    public User(string name, string email, string passwordHash)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        FailedSignInAttempts = 0;
        LockoutEndOnUtc = null;
        LastPasswordUpdateOnUtc = null;
        MagicSignInId = null;
    }

    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public int FailedSignInAttempts { get; private set; }
    public DateTime? LastPasswordUpdateOnUtc { get; private set; }
    public DateTime? LockoutEndOnUtc { get; private set; }
    public Guid? MagicSignInId { get; private set; }

    public void IncrementFailedSignInAttempts()
    {
        SetUpdateOnToUtcNow();
        FailedSignInAttempts++;
    }

    public void SetLockoutEnd(DateTime lockoutEnd)
    {
        SetUpdateOnToUtcNow();
        LockoutEndOnUtc = lockoutEnd;
    }

    public void ResetLockout()
    {
        SetUpdateOnToUtcNow();
        FailedSignInAttempts = 0;
        LockoutEndOnUtc = null;
    }

    public void UpdatePasswordHash(string passwordHash)
    {
        var utcNow = DateTime.UtcNow;
        SetUpdatedOnTo(utcNow);

        PasswordHash = passwordHash;
        LastPasswordUpdateOnUtc = utcNow;
    }

    public void SetMagicSignInId(Guid magicSignInId)
    {
        SetUpdateOnToUtcNow();
        MagicSignInId = magicSignInId;
    }

    public void ResetMagicSignInId()
    {
        SetUpdateOnToUtcNow();
        MagicSignInId = null;
    }
}