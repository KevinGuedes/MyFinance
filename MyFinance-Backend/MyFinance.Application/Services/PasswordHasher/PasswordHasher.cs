using BC = BCrypt.Net.BCrypt;

namespace MyFinance.Application.Services.PasswordHasher;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string plainTextPassword)
        => BC.EnhancedHashPassword(plainTextPassword, 14);

    public bool VerifyHashedPassword(string plainTextPassword, string passwordHash)
        => BC.EnhancedVerify(plainTextPassword, passwordHash);
}
