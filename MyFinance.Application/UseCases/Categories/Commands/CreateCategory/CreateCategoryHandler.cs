using FluentResults;
using Microsoft.EntityFrameworkCore;
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
        var isValidManagementUnit = await _myFinanceDbContext.ManagementUnits
            .AnyAsync(mu => mu.Id == command.ManagementUnitId, cancellationToken);

        if (!isValidManagementUnit)
        {
            var errorMessage = $"Management Unit with Id {command.ManagementUnitId} not found";
            var entityNotFoundError = new EntityNotFoundError(errorMessage);
            return Result.Fail(entityNotFoundError);
        }

        var category = new Category(command.Name, command.ManagementUnitId, command.CurrentUserId);
        await _myFinanceDbContext.Categories.AddAsync(category, cancellationToken);

        return Result.Ok(new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
        });
    }
}