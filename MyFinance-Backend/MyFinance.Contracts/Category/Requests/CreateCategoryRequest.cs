namespace MyFinance.Contracts.Category.Requests;

public sealed class CreateCategoryRequest
{
    public required string Name { get; init; } = null!;
}