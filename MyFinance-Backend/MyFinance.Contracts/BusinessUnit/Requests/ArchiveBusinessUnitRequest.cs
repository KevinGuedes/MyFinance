namespace MyFinance.Contracts.BusinessUnit.Requests;

public sealed class ArchiveBusinessUnitRequest
{
    public required Guid Id { get; init; }
    public required string? ReasonToArchive { get; init; }
}
