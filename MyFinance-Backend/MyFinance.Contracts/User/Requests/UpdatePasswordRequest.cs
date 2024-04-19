namespace MyFinance.Contracts.User.Requests;

public sealed record UpdatePasswordRequest(
    string PlainTextCurrentPassword,
    string PlainTextNewPassword,
    string PlainTextNewPasswordConfirmation)
{
}
