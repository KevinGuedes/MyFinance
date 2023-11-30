using FluentValidation;

namespace MyFinance.Application.UseCases.AccountTags.Commands.ArchiveAccountTag;

public sealed class ArchiveAccountTagValidator : AbstractValidator<ArchiveAccountTagCommand>
{
    public ArchiveAccountTagValidator()
    {
        RuleFor(command => command.ReasonToArchive)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .Length(10, 200).WithMessage("{PropertyName} must have between 10 and 200 characters");

        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");
    }
}
