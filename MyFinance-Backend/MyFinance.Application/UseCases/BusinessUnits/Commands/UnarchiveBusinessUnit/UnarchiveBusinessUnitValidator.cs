using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UnarchiveBusinessUnit;

public sealed class UnarchiveBusinessUnitValidator : AbstractValidator<UnarchiveBusinessUnitCommand>
{
    public UnarchiveBusinessUnitValidator()
    {
        RuleFor(command => command.Id).MustBeAValidGuid();
    }
}