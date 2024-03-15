using FluentValidation;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UnarchiveBusinessUnit;

public sealed class UnarchiveBusinessUnitValidator : AbstractValidator<UnarchiveBusinessUnitCommand>
{
    public UnarchiveBusinessUnitValidator()
    {
        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");
    }
}