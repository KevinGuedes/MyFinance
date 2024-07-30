using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.Category.Requests;
using MyFinance.Contracts.Category.Responses;

namespace MyFinance.Application.UseCases.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommand(CreateCategoryRequest request)
    : IUserRequiredRequest, ICommand<CategoryResponse>
{
    public Guid CurrentUserId { get; set; }
    public Guid ManagementUnitId { get; init; } = request.ManagementUnitId;
    public string Name { get; init; } = request.Name;
}