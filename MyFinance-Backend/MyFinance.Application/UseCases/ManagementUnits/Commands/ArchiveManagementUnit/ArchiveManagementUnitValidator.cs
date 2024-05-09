using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.ArchiveManagementUnit;

public sealed class ArchiveManagementUnitValidator : AbstractValidator<ArchiveManagementUnitCommand>
{
    public ArchiveManagementUnitValidator()
    {
        RuleFor(command => command.ReasonToArchive)
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Id).MustBeAValidGuid();
    }
}