namespace MyFinance.Contracts.Category.Responses;

public sealed class CategoryResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required bool IsArchived { get; init; }
    public required string? ReasonToArchive { get; init; }
}
