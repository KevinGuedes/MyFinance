namespace MyFinance.Contracts.Category.Requests;

public sealed record UpdateCategoryRequest(Guid Id, string Name)
{
}