namespace MyFinance.Contracts.BusinessUnit.Requests;

public sealed record CreateBusinessUnitRequest(string Name, string? Description)
{
}