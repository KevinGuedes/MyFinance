namespace MyFinance.Contracts.ManagementUnit.Requests;

public sealed record CreateManagementUnitRequest(string Name, string? Description)
{
}