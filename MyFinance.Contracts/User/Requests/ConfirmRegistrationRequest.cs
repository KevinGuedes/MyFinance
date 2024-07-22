namespace MyFinance.Contracts.User.Requests;

public sealed record ConfirmRegistrationRequest(string UrlSafeConfirmRegistrationToken)
{
}
