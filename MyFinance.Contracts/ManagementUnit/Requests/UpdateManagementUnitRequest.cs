namespace MyFinance.Contracts.ManagementUnit.Requests;

public sealed record UpdateManagementUnitRequest(Guid Id, string Name, string? Description)
{
}