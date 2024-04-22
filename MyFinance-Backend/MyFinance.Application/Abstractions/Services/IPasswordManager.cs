namespace MyFinance.Application.Abstractions.Services;

public interface IPasswordManager
{
    string HashPassword(string plainTextPassword);
    bool VerifyPassword(string plainTextPassword, string passwordHash);
    bool ShouldUpdatePassword(DateTime lastPasswordUpdateOnUtc);
    bool ArePasswordsSimilar(string plainTextPassword, string plainTextNewPassword);
}