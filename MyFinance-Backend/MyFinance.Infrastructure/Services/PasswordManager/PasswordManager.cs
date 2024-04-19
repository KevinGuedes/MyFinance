using F23.StringSimilarity;
using Microsoft.Extensions.Options;
using MyFinance.Application.Abstractions.Services;
using BC = BCrypt.Net.BCrypt;

namespace MyFinance.Infrastructure.Services.PasswordManager;

public sealed class PasswordManager : IPasswordManager
{
    private readonly PasswordManagerOptions _passwordManagerOptions;

    public PasswordManager(IOptions<PasswordManagerOptions> passwordManagerOptions)
    {
        _passwordManagerOptions = passwordManagerOptions.Value;

        ArgumentNullException.ThrowIfNull(
            _passwordManagerOptions.WorkFactor, 
            nameof(_passwordManagerOptions.WorkFactor));
        
        if (_passwordManagerOptions.WorkFactor < _passwordManagerOptions.MinimumAllowedWorkFactor)
            throw new ArgumentException($"Work factor must be equal or greater than {_passwordManagerOptions.MinimumAllowedWorkFactor}");
    }

    public bool ArePasswordsSimilar(string plainTextPassword, string plainTextNewPassword)
    {
        var cosineSimilarityAlgorithm = new Cosine(2);
        var similarity = cosineSimilarityAlgorithm.Similarity(plainTextPassword, plainTextNewPassword);
        return similarity > _passwordManagerOptions.SimilarityThreshold;
    }

    public string HashPassword(string plainTextPassword)
        => BC.EnhancedHashPassword(plainTextPassword, _passwordManagerOptions.WorkFactor);

    public bool VerifyPassword(string plainTextPassword, string passwordHash)
        => BC.EnhancedVerify(plainTextPassword, passwordHash);
}