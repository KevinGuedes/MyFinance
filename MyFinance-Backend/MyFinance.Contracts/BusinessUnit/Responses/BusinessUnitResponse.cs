namespace MyFinance.Contracts.BusinessUnit.Responses;

public sealed class BusinessUnitResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required decimal Income { get; init; }
    public required decimal Outcome { get; init; }
    public required decimal Balance { get; init; }
    public required string? Description { get; init; }
    public required bool IsArchived { get; init; }
    public required string? ReasonToArchive { get; init; }
}