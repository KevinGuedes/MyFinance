using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Services;

public interface IPasswordManager
{
    string HashPassword(string plainTextPassword);
    bool VerifyPassword(string plainTextPassword, string passwordHash);
    bool ShouldUpdatePassword(User user);
    bool ArePasswordsSimilar(string plainTextPassword, string plainTextNewPassword);
}