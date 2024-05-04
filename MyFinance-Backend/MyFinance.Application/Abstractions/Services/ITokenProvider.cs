namespace MyFinance.Application.Abstractions.Services;

public interface ITokenProvider
{
    string CreateUrlSafeConfirmRegistrationToken(Guid userId);
    string CreateUrlSafeMagicSignInToken(Guid userId);
    string CreateUrlSafeResetPasswordToken(Guid userId);
    bool TryGetUserIdFromUrlSafeConfirmRegistrationToken(
        string urlSafeConfirmRegistrationToken,
        out Guid userId);
    bool TryGetUserIdFromUrlSafeMagicSignInToken(
        string urlSafeMagicSignInToken,
        out Guid userId);
    bool TryGetUserIdFromUrlSafeResetPasswordToken(
        string urlSafeResetPasswordToken,
        out Guid userId);
}
