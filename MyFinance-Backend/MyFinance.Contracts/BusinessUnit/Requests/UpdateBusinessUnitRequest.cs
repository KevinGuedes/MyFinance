namespace MyFinance.Contracts.BusinessUnit.Requests;

public sealed class UpdateBusinessUnitRequest
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
}