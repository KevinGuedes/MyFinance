namespace MyFinance.Contracts.User.Requests;

public sealed record ResetPasswordRequest(
    string PlainTextNewPassword,
    string PlainTextNewPasswordConfirmation)
{
}
