namespace MyFinance.Contracts.User.Requests;

public sealed record ResetPasswordRequest(
    string UrlSafeResetPasswordToken,
    string PlainTextNewPassword,
    string PlainTextNewPasswordConfirmation)
{
}
