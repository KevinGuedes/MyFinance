using Microsoft.Extensions.Options;
using MyFinance.Application.Abstractions.Services;
using BC = BCrypt.Net.BCrypt;

namespace MyFinance.Infrastructure.Services.PasswordHasher;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasherOptions _passwordHasherOptions;

    public PasswordHasher(IOptions<PasswordHasherOptions> passwordHasherOptions)
    {
        _passwordHasherOptions = passwordHasherOptions.Value;

        ArgumentNullException.ThrowIfNull(_passwordHasherOptions.WorkFactor);
        ArgumentNullException.ThrowIfNull(_passwordHasherOptions.MinimumAllowedWorkFactor);
        if (_passwordHasherOptions.WorkFactor < _passwordHasherOptions.MinimumAllowedWorkFactor)
            throw new ArgumentException("Work factor must be equal or greater than 14");
    }

    public string HashPassword(string plainTextPassword)
        => BC.EnhancedHashPassword(plainTextPassword, _passwordHasherOptions.WorkFactor);

    public bool VerifyPassword(string plainTextPassword, string passwordHash)
        => BC.EnhancedVerify(plainTextPassword, passwordHash);
}