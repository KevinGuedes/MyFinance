using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.Category.Requests;
using MyFinance.Contracts.Category.Responses;

namespace MyFinance.Application.UseCases.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommand(UpdateCategoryRequest request)
    : ICommand<CategoryResponse>
{
    public Guid Id { get; init; } = request.Id;
    public string Name { get; init; } = request.Name;
}