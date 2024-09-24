using Microsoft.Extensions.Options;
using MyFinance.Application.Abstractions.Services;
using Polly;
using Polly.Retry;

namespace MyFinance.Infrastructure.Services.EmailSender;

internal sealed class EmailSender(IOptions<EmailSenderOptions> emailSenderOptions) : IEmailSender
{
    private readonly EmailSenderOptions _emailSenderOptions = emailSenderOptions.Value;

    public bool IsEmailConfirmationEnabled => _emailSenderOptions.UseEmailConfirmation;

    private readonly AsyncRetryPolicy _sendEmailRetryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(
            3,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    public Task<(bool HasEmailBeenSent, Exception? Exception)> SendConfirmRegistrationEmailAsync(
        string email,
        string urlSafeConfirmRegistrationToken,
        CancellationToken cancellationToken)
        => SendEmailAsync(email, urlSafeConfirmRegistrationToken, cancellationToken);

    public Task<(bool HasEmailBeenSent, Exception? Exception)> SendMagicSignInEmailAsync(
        string email,
        string urlSafeMagicSignInToken,
        CancellationToken cancellationToken)
        => SendEmailAsync(email, urlSafeMagicSignInToken, cancellationToken);

    public Task<(bool HasEmailBeenSent, Exception? Exception)> SendResetPasswordEmailAsync(
        string email,
        string urlSafeResetPasswordToken,
        CancellationToken cancellationToken)
        => SendEmailAsync(email, urlSafeResetPasswordToken, cancellationToken);

    public Task<(bool HasEmailBeenSent, Exception? Exception)> SendUserLockedEmailAsync(
        string email,
        TimeSpan lockoutDuration,
        DateTime lockoutEndOnUtc,
        CancellationToken cancellationToken)
        => SendEmailAsync(email, cancellationToken);

    private async Task<(bool HasEmailBeenSent, Exception? Exception)> SendEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {

        var result = await _sendEmailRetryPolicy.ExecuteAndCaptureAsync((cancellationToken) =>
        {
            return Task.CompletedTask;
        }, cancellationToken);

        return ProcessResult(result);
    }

    private async Task<(bool HasEmailBeenSent, Exception? Exception)> SendEmailAsync(
        string email,
        string urlSafeToken,
        CancellationToken cancellationToken)
    {
        var result = await _sendEmailRetryPolicy.ExecuteAndCaptureAsync((cancellationToken) =>
        {
            return Task.CompletedTask;
        }, cancellationToken);

        return ProcessResult(result);
    }

    private static (bool HasEmailBeenSent, Exception? Exception) ProcessResult(PolicyResult result)
        => (HasEmailBeenSent: result.Outcome == OutcomeType.Successful, Exception: result.FinalException);
}
