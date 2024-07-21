namespace MyFinance.Contracts.AccountTag.Requests;

public sealed record UpdateAccountTagRequest(Guid Id, string Tag, string? Description)
{
}