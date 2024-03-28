using FluentValidation;
using MyFinance.Application.Abstractions.Persistence.Repositories;

namespace MyFinance.Application.UseCases.Categories.Commands.CreateCategory;

public sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.CurrentUserId)
           .NotEqual(Guid.Empty).WithMessage("Invalid {PropertyName}");

        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .Length(3, 50).WithMessage("{PropertyName} must have between 3 and 50 characters")
            .MustAsync(async (name, cancellationToken) =>
            {
                var exists = await _categoryRepository.ExistsByNameAsync(name, cancellationToken);
                return !exists;
            }).WithMessage("This {PropertyName} has already been taken");
    }
}