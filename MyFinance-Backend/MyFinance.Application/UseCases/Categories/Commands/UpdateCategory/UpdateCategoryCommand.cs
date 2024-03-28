using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.Category.Responses;

namespace MyFinance.Application.UseCases.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(Guid Id, string Name)
    : ICommand<CategoryResponse>
{
}