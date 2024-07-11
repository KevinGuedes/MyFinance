using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.Category.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.Categories.Commands.CreateCategory;

internal sealed class CreateCategoryHandler(
    ICategoryRepository categoryRepository,
    IManagementUnitRepository managementUnitRepository)
    : ICommandHandler<CreateCategoryCommand, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IManagementUnitRepository _managementUnitRepository = managementUnitRepository;

    public async Task<Result<CategoryResponse>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var managementUnit = await _managementUnitRepository.GetByIdAsync(command.ManagementUnitId, cancellationToken);
        if (managementUnit is null)
        {
            var errorMessage = $"Management Unit with Id {command.ManagementUnitId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var category = new Category(managementUnit, command.Name, command.CurrentUserId);
        await _categoryRepository.InsertAsync(category, cancellationToken);

        return Result.Ok(CategoryMapper.DTR.Map(category));
    }
}