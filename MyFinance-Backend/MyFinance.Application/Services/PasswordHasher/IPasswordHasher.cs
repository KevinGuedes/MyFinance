namespace MyFinance.Application.Services.PasswordHasher;

public interface IPasswordHasher
{
    string HashPassword(string plainTextPassword);
    bool VerifyHashedPassword(string plainTextPassword, string passwordHash);
}
