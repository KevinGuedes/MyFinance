using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;
using MyFinance.Contracts.Category.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.Categories.Commands.CreateCategory;

internal sealed class CreateCategoryHandler(IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<CreateCategoryCommand, CategoryResponse>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<CategoryResponse>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var managementUnit = await _myFinanceDbContext.ManagementUnits
            .FindAsync([command.ManagementUnitId], cancellationToken);

        if (managementUnit is null)
        {
            var errorMessage = $"Management Unit with Id {command.ManagementUnitId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var category = new Category(managementUnit, command.Name, command.CurrentUserId);
        await _myFinanceDbContext.Categories.AddAsync(category, cancellationToken);

        return Result.Ok(new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
        });
    }
}