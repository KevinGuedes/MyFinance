namespace MyFinance.Contracts.ManagementUnit.Responses;

public sealed class ManagementUnitResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required decimal Income { get; init; }
    public required decimal Outcome { get; init; }
    public decimal Balance => Income - Outcome;
    public required string? Description { get; init; }
}