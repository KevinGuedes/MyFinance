namespace MyFinance.Application.Abstractions.Services;

public interface IEmailSender
{
    Task<(bool HasEmailBeenSent, Exception? Exception)> SendConfirmRegistrationEmailAsync(
        string email, 
        string urlSafeConfirmRegistrationToken);
    Task<(bool HasEmailBeenSent, Exception? Exception)> SendMagicSignInEmailAsync(
        string email, 
        string urlSafeMagicSignInToken);
    Task<(bool HasEmailBeenSent, Exception? Exception)> SendResetPasswordEmailAsync(
        string email, 
        string urlSafeResetPasswordToken);
}
