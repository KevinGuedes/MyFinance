using MyFinance.Application.Abstractions.Services;
using Polly;
using Polly.Retry;

namespace MyFinance.Infrastructure.Services.EmailSender;

internal sealed class EmailSender : IEmailSender
{
    private readonly AsyncRetryPolicy _sendEmailRetryPolicy;

    public EmailSender()
    {
        _sendEmailRetryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt
                => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public async Task<(bool HasEmailBeenSent, Exception? Exception)> SendConfirmRegistrationEmailAsync(
        string email,
        string urlSafeConfirmRegistrationToken)
    {
        var result = await _sendEmailRetryPolicy.ExecuteAndCaptureAsync(()
            => SendEmailAsync(email, urlSafeConfirmRegistrationToken));

        return ProcessResult(result);
    }

    public async Task<(bool HasEmailBeenSent, Exception? Exception)> SendMagicSignInEmailAsync(
        string email,
        string urlSafeMagicSignInToken)
    {
        var result = await _sendEmailRetryPolicy.ExecuteAndCaptureAsync(()
            => SendEmailAsync(email, urlSafeMagicSignInToken));

        return ProcessResult(result);
    }

    public async Task<(bool HasEmailBeenSent, Exception? Exception)> SendResetPasswordEmailAsync(
        string email,
        string urlSafeResetPasswordToken)
    {
        var result = await _sendEmailRetryPolicy.ExecuteAndCaptureAsync(()
            => SendEmailAsync(email, urlSafeResetPasswordToken));

        return ProcessResult(result);
    }

    private static Task SendEmailAsync(string email, string urlSafeToken)
    {
        return Task.CompletedTask;
    }

    private static (bool HasEmailBeenSent, Exception? Exception) ProcessResult(PolicyResult result)
    {
        var hasEmailBeenSent = result.Outcome == OutcomeType.Successful;
        return (HasEmailBeenSent: hasEmailBeenSent, Exception: result.FinalException);
    }
}
