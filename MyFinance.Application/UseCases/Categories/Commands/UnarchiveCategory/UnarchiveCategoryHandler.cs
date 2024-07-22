using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.Categories.Commands.UnarchiveCategory;

internal sealed class UnarchiveCategoryHandler(ICategoryRepository categoryRepository)
    : ICommandHandler<UnarchiveCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<Result> Handle(UnarchiveCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.Id, cancellationToken);

        if (category is null)
        {
            var entityNotFoundError = new EntityNotFoundError($"Category with Id {command.Id} not found");
            return Result.Fail(entityNotFoundError);
        }

        category.Unarchive();
        _categoryRepository.Update(category);

        return Result.Ok();
    }
}