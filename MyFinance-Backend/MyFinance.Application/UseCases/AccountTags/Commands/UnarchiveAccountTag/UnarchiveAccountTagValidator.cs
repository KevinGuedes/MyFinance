using FluentValidation;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UnarchiveAccountTag;

public sealed class UnarchiveAccountTagValidator : AbstractValidator<UnarchiveAccountTagCommand>
{
    public UnarchiveAccountTagValidator()
    {
        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");
    }
}