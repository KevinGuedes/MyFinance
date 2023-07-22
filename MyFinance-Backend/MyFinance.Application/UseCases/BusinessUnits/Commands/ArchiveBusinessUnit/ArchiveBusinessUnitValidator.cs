using FluentValidation;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.ArchiveBusinessUnit;

public sealed class ArchiveBusinessUnitValidator : AbstractValidator<ArchiveBusinessUnitCommand>
{
    private readonly IBusinessUnitRepository _businessUnitRepository;

    public ArchiveBusinessUnitValidator(IBusinessUnitRepository businessUnitRepository)
    {
        _businessUnitRepository = businessUnitRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.ReasonToArchive)
          .Cascade(CascadeMode.Stop)
          .NotNull().WithMessage("{PropertyName} must not be null")
          .NotEmpty().WithMessage("{PropertyName} must not be empty")
          .MinimumLength(10).WithMessage("{PropertyName} must have 10 or more characters");

        //RuleFor(command => command.Id)
        //    .Cascade(CascadeMode.Stop)
        //    .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid")
        //    .MustAsync(async (businessUnitId, cancellationToken) =>
        //    {
        //        var exists = await _businessUnitRepository.ExistsByIdAsync(businessUnitId, cancellationToken);
        //        return exists;
        //    }).WithMessage("Business Unit not found");
    }
}
