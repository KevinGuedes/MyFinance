using F23.StringSimilarity;
using Microsoft.Extensions.Options;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Entities;
using BC = BCrypt.Net.BCrypt;

namespace MyFinance.Infrastructure.Services.PasswordManager;

internal sealed class PasswordManager(IOptions<PasswordOptions> passwordOptions) : IPasswordManager
{
    private readonly PasswordOptions _passwordOptions = passwordOptions.Value;

    public bool ShouldUpdatePassword(User user)
        => user.LastPasswordUpdateOnUtc is null ?
            ShouldUpdatePassword(user.CreatedOnUtc) :
            ShouldUpdatePassword(user.LastPasswordUpdateOnUtc.Value);

    private bool ShouldUpdatePassword(DateTime lastPasswordUpdateOnUtc)
        => DateTime.UtcNow > lastPasswordUpdateOnUtc.AddMonths(_passwordOptions.TimeInMonthsToRequestPasswordUpdate);

    public bool ArePasswordsSimilar(string plainTextPassword, string plainTextNewPassword)
    {
        var cosineSimilarityAlgorithm = new Cosine(2);
        var similarity = cosineSimilarityAlgorithm.Similarity(plainTextPassword, plainTextNewPassword);
        return similarity > _passwordOptions.SimilarityThreshold;
    }

    public string HashPassword(string plainTextPassword)
        => BC.EnhancedHashPassword(plainTextPassword, _passwordOptions.WorkFactor);

    public bool VerifyPassword(string plainTextPassword, string passwordHash)
        => BC.EnhancedVerify(plainTextPassword, passwordHash);
}