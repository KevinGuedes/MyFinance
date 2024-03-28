using FluentValidation;

namespace MyFinance.Application.UseCases.Categories.Commands.UnarchiveCategory;

public sealed class UnarchiveCategoryValidator : AbstractValidator<UnarchiveCategoryCommand>
{
    public UnarchiveCategoryValidator()
    {
        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");
    }
}