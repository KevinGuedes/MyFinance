using FluentValidation;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Id).MustBeAValidGuid();

        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .Length(3, 50).WithMessage("{PropertyName} must have between 3 and 50 characters")
            .MustAsync(async (command, name, cancellationToken) =>
            {
                var existingCategory = await _categoryRepository.GetByNameAsync(name, cancellationToken);
                if (existingCategory is null)
                    return true;

                var isValid = existingCategory.Id == command.Id;
                return isValid;
            }).WithMessage("This {PropertyName} has already been taken");
    }
}