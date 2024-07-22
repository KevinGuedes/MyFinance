using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.Categories.Commands.UnarchiveCategory;

public sealed class UnarchiveCategoryValidator : AbstractValidator<UnarchiveCategoryCommand>
{
    public UnarchiveCategoryValidator()
    {
        RuleFor(command => command.Id).MustBeAValidGuid();
    }
}