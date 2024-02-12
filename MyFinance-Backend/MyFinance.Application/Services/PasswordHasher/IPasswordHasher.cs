namespace MyFinance.Application.Services.PasswordHasher;

public interface IPasswordHasher
{
    string HashPassword(string plainTextPassword);
    bool VerifyPassword(string plainTextPassword, string passwordHash);
}
