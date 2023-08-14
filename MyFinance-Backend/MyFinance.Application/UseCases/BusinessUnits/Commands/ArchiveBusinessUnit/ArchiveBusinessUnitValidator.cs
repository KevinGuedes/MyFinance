using FluentValidation;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;

public sealed class ArchiveBusinessUnitValidator : AbstractValidator<ArchiveBusinessUnitCommand>
{
    public ArchiveBusinessUnitValidator()
    {
        RuleFor(command => command.ReasonToArchive)
          .NotNull().WithMessage("{PropertyName} must not be null")
          .NotEmpty().WithMessage("{PropertyName} must not be empty")
          .Length(10, 140).WithMessage("{PropertyName} must have between 10 and 140 characters");

        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");
    }
}
