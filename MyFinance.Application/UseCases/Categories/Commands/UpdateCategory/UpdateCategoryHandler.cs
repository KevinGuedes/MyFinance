using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;
using MyFinance.Contracts.Category.Responses;

namespace MyFinance.Application.UseCases.Categories.Commands.UpdateCategory;

internal sealed class UpdateCategoryHandler(IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<UpdateCategoryCommand, CategoryResponse>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result<CategoryResponse>> Handle(UpdateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var category = await _myFinanceDbContext.Categories.FindAsync([command.Id], cancellationToken);

        if (category is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Category with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        category.Update(command.Name);
        _myFinanceDbContext.Categories.Update(category);

        return Result.Ok(new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
        });
    }
}