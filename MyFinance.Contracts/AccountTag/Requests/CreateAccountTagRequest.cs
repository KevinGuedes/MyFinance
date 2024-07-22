namespace MyFinance.Contracts.AccountTag.Requests;

public sealed record CreateAccountTagRequest(Guid ManagementUnitId, string Tag, string? Description)
{
}