namespace MyFinance.Contracts.Category.Requests;

public sealed record CreateCategoryRequest(Guid ManagementUnitId, string Name)
{
}