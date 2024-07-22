namespace MyFinance.Application.Abstractions.Services;

public interface IEmailSender
{
    Task<(bool HasEmailBeenSent, Exception? Exception)> SendConfirmRegistrationEmailAsync(
        string email,
        string urlSafeConfirmRegistrationToken,
        CancellationToken cancellationToken);
    Task<(bool HasEmailBeenSent, Exception? Exception)> SendMagicSignInEmailAsync(
        string email,
        string urlSafeMagicSignInToken,
        CancellationToken cancellationToken);
    Task<(bool HasEmailBeenSent, Exception? Exception)> SendResetPasswordEmailAsync(
        string email,
        string urlSafeResetPasswordToken,
        CancellationToken cancellationToken);
    Task<(bool HasEmailBeenSent, Exception? Exception)> SendUserLockedEmailAsync(
        string email,
        TimeSpan lockoutDuration,
        DateTime lockoutEndOnUtc,
        CancellationToken cancellationToken);
}
