using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.UnarchiveManagementUnit;

public sealed class UnarchiveManagementUnitValidator : AbstractValidator<UnarchiveManagementUnitCommand>
{
    public UnarchiveManagementUnitValidator()
    {
        RuleFor(command => command.Id).MustBeAValidGuid();
    }
}