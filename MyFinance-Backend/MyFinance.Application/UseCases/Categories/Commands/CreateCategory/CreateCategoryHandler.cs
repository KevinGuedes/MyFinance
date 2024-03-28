using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Category.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.Categories.Commands.CreateCategory;

internal sealed class CreateCategoryHandler(ICategoryRepository categoryRepository)
    : ICommandHandler<CreateCategoryCommand, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public Task<Result<CategoryResponse>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = new Category(command.Name, command.CurrentUserId);
        _categoryRepository.Insert(category);

        var result = Result.Ok(CategoryMapper.DTR.Map(category));
        return Task.FromResult(result);
    }
}