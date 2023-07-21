using FluentValidation;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UnarchiveBusinessUnit;

public sealed class UnarchiveBusinessUnitValidator : AbstractValidator<UnarchiveBusinessUnitCommand>
{
    private readonly IBusinessUnitRepository _businessUnitRepository;

    public UnarchiveBusinessUnitValidator(IBusinessUnitRepository businessUnitRepository)
    {
        _businessUnitRepository = businessUnitRepository;

        RuleFor(command => command.Id)
           .Cascade(CascadeMode.Stop)
           .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid")
           .MustAsync(async (businessUnitId, cancellationToken) =>
           {
               var exists = await _businessUnitRepository.ExistsByIdAsync(businessUnitId, cancellationToken);
               return exists;
           }).WithMessage("Business Unit not found");
    }
}