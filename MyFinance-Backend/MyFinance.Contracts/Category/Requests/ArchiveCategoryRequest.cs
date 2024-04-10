namespace MyFinance.Contracts.Category.Requests;

public sealed class ArchiveCategoryRequest
{
    public required Guid Id { get; init; }
    public required string? ReasonToArchive { get; init; }
}