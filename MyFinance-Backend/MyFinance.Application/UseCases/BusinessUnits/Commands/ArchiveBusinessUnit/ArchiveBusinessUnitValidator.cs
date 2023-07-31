using FluentValidation;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;

public sealed class ArchiveBusinessUnitValidator : AbstractValidator<ArchiveBusinessUnitCommand>
{
    public ArchiveBusinessUnitValidator()
    {
        RuleFor(command => command.ReasonToArchive)
          .NotNull().WithMessage("{PropertyName} must not be null")
          .NotEmpty().WithMessage("{PropertyName} must not be empty")
          .MinimumLength(10).WithMessage("{PropertyName} must have 10 or more characters");

        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");
    }
}
