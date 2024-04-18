namespace MyFinance.Contracts.BusinessUnit.Requests;

public sealed record UpdateBusinessUnitRequest(Guid Id, string Name, string? Description)
{
}