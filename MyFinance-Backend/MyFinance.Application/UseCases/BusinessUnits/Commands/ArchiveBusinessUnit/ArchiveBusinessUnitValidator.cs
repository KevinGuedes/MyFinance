using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;

public sealed class ArchiveBusinessUnitValidator : AbstractValidator<ArchiveBusinessUnitCommand>
{
    public ArchiveBusinessUnitValidator()
    {
        RuleFor(command => command.ReasonToArchive)
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Id).MustBeAValidGuid();
    }
}