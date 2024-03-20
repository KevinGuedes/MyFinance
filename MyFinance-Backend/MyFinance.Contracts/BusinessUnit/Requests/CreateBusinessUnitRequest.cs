namespace MyFinance.Contracts.BusinessUnit.Requests;

public sealed class CreateBusinessUnitRequest
{
    public required string Name { get; init; }
    public required string? Description { get; init; }
}