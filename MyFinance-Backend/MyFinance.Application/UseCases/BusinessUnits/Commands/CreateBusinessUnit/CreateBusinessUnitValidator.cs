using FluentValidation;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;

public sealed class CreateBusinessUnitValidator : AbstractValidator<CreateBusinessUnitCommand>
{
    private readonly IBusinessUnitRepository _businessUnitRepository;

    public CreateBusinessUnitValidator(IBusinessUnitRepository businessUnitRepository)
    {
        _businessUnitRepository = businessUnitRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        When(command => command.Description is not null, () =>
        {
            RuleFor(command => command.Description)
                .NotNull().WithMessage("{PropertyName} must not be null")
                .NotEmpty().WithMessage("{PropertyName} must not be empty")
                .Length(10, 200).WithMessage("{PropertyName} must have between 10 and 200 characters");
        });

        RuleFor(command => command.Name)
           .Cascade(CascadeMode.Stop)
           .NotNull().WithMessage("{PropertyName} must not be null")
           .NotEmpty().WithMessage("{PropertyName} must not be empty")
           .Length(3, 30).WithMessage("{PropertyName} must have between 3 and 30 characters")
           .MustAsync(async (businessUnitName, cancellationToken) =>
           {
               var exists = await _businessUnitRepository.ExistsByNameAsync(businessUnitName, cancellationToken);
               return !exists;
           }).WithMessage("This {PropertyName} has already been taken");
    }
}
