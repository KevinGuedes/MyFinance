namespace MyFinance.Contracts.AccountTag;

public sealed record UpdateAccountTagRequest(Guid Id, string Tag, string? Description)
{
}