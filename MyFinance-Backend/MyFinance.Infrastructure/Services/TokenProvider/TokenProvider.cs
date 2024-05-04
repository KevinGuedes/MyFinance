using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using MyFinance.Application.Abstractions.Services;
using System.Net;
using System.Security.Cryptography;

namespace MyFinance.Infrastructure.Services.TokenProvider;

internal sealed class TokenProvider(
    IOptions<TokenOptions> tokenOptions,
    IDataProtectionProvider idp) : ITokenProvider
{
    private readonly TokenOptions _tokenOptions = tokenOptions.Value;
    private readonly IDataProtectionProvider _idp = idp;

    public string CreateUrlSafeConfirmRegistrationToken(Guid userId)
        => CreateUrlSafeToken(_tokenOptions.ConfirmRegistrationTokenPurposes, userId);

    public string CreateUrlSafeMagicSignInToken(Guid userId)
        => CreateTimeLimitedUrlSafeToken(_tokenOptions.MagicSignInTokenPurposes, userId);

    public string CreateUrlSafeResetPasswordToken(Guid userId)
        => CreateTimeLimitedUrlSafeToken(_tokenOptions.ResetPasswordTokenPurposes, userId);

    public bool TryGetUserIdFromUrlSafeConfirmRegistrationToken(
        string urlSafeConfirmRegistration,
        out Guid userId)
        => TryGetUserIdFromUrlSafeToken(
            urlSafeConfirmRegistration,
            _tokenOptions.ConfirmRegistrationTokenPurposes,
            out userId);

    public bool TryGetUserIdFromUrlSafeMagicSignInToken(
        string urlSafeMagicSignInToken,
        out Guid userId)
        => TryGetUserIdFromTimeLimitedUrlSafeToken(
            urlSafeMagicSignInToken,
            _tokenOptions.MagicSignInTokenPurposes,
            out userId);

    public bool TryGetUserIdFromUrlSafeResetPasswordToken(
        string urlSafeResetPasswordToken,
        out Guid userId)
        => TryGetUserIdFromTimeLimitedUrlSafeToken(
            urlSafeResetPasswordToken,
            _tokenOptions.ResetPasswordTokenPurposes,
            out userId);

    private string CreateUrlSafeToken(string[] purposes, Guid userId)
    {
        var dp = _idp.CreateProtector(purposes);
        var token = dp.Protect(userId.ToString());
        return WebUtility.UrlEncode(token);
    }

    private bool TryGetUserIdFromUrlSafeToken(
        string urlSafeToken,
        string[] purposes,
        out Guid userId)
    {
        var dp = _idp.CreateProtector(purposes);

        try
        {
            var protectedData = WebUtility.UrlDecode(urlSafeToken);
            return Guid.TryParse(dp.Unprotect(protectedData), out userId);
        }
        catch (CryptographicException)
        {
            userId = Guid.Empty;
            return false;
        }
    }

    private string CreateTimeLimitedUrlSafeToken(string[] purposes, Guid userId)
    {
        var tldp = _idp.CreateProtector(purposes).ToTimeLimitedDataProtector();

        var timeLimitedToken = tldp.Protect(
            userId.ToString(),
            TimeSpan.FromMinutes(_tokenOptions.DefaultTokenDurationInMinutes));

        return WebUtility.UrlEncode(timeLimitedToken);
    }

    private bool TryGetUserIdFromTimeLimitedUrlSafeToken(
        string timeLimitedUrlSafeToken,
        string[] purposes,
        out Guid userId)
    {
        var tldp = _idp.CreateProtector(purposes).ToTimeLimitedDataProtector();

        try
        {
            var protectedData = WebUtility.UrlDecode(timeLimitedUrlSafeToken);
            return Guid.TryParse(tldp.Unprotect(protectedData), out userId);
        }
        catch (CryptographicException)
        {
            userId = Guid.Empty;
            return false;
        }
    }
}
