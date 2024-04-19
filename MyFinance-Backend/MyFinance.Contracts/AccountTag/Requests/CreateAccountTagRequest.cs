namespace MyFinance.Contracts.AccountTag.Requests;

public sealed record CreateAccountTagRequest(string Tag, string? Description)
{
}