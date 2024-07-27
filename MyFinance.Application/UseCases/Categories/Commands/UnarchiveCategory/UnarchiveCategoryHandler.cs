using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.Categories.Commands.UnarchiveCategory;

internal sealed class UnarchiveCategoryHandler(IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<UnarchiveCategoryCommand>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result> Handle(UnarchiveCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _myFinanceDbContext.Categories.FindAsync([command.Id], cancellationToken);

        if (category is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Category with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        category.Unarchive();
        _myFinanceDbContext.Categories.Update(category);

        return Result.Ok();
    }
}