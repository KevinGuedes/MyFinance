using F23.StringSimilarity;
using Microsoft.Extensions.Options;
using MyFinance.Application.Abstractions.Services;
using BC = BCrypt.Net.BCrypt;

namespace MyFinance.Infrastructure.Services.PasswordManager;

internal sealed class PasswordManager : IPasswordManager
{
    private readonly PasswordOptions _passwordOptions;

    public PasswordManager(IOptions<PasswordOptions> passwordOptions)
    {
        _passwordOptions = passwordOptions.Value;


        if (_passwordOptions.TimeInMonthsToRequestPasswordUpdate > _passwordOptions.MaximumAllowedTimeInMonthsToRequestPasswordUpdate)
        {
            var message = "Time in months to request password update must be equal " + 
                $"or less than {_passwordOptions.MaximumAllowedTimeInMonthsToRequestPasswordUpdate}";

            throw new ArgumentException(message);
        }

        if (_passwordOptions.WorkFactor < _passwordOptions.MinimumAllowedWorkFactor)
            throw new ArgumentException($"Work factor must be equal or greater than {_passwordOptions.MinimumAllowedWorkFactor}");
    }
    public bool ShouldUpdatePassword(DateTime lastPasswordUpdateOnUtc)
        => DateTime.UtcNow > 
            lastPasswordUpdateOnUtc.AddMonths(_passwordOptions.TimeInMonthsToRequestPasswordUpdate);


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