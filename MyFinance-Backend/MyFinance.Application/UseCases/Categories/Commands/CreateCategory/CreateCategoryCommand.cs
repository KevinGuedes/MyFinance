using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.Category.Responses;

namespace MyFinance.Application.UseCases.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(string Name)
    : IUserRequiredRequest, ICommand<CategoryResponse>
{
    public Guid CurrentUserId { get; set; }
}