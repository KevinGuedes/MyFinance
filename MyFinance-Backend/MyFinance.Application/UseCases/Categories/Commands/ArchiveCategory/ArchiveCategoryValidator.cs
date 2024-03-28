using FluentValidation;

namespace MyFinance.Application.UseCases.Categories.Commands.ArchiveCategory;

public sealed class ArchiveCategoryValidator : AbstractValidator<ArchiveCategoryCommand>
{
    public ArchiveCategoryValidator()
    {
        RuleFor(command => command.ReasonToArchive)
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");
    }
}