using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UnarchiveAccountTag;

public sealed class UnarchiveAccountTagValidator : AbstractValidator<UnarchiveAccountTagCommand>
{
    public UnarchiveAccountTagValidator()
    {
        RuleFor(command => command.Id).MustBeAValidGuid();
    }
}