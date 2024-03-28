namespace MyFinance.Contracts.Category.Requests;

public sealed class UpdateCategoryRequest
{
    public required Guid Id { get; init; }
    public required string Name { get; init; } = null!;
}