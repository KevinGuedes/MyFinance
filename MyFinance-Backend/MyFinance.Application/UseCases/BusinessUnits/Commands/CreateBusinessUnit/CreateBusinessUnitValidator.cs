using FluentValidation;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;

public sealed class CreateBusinessUnitValidator : AbstractValidator<CreateBusinessUnitCommand>
{
    private readonly IBusinessUnitRepository _businessUnitRepository;

    public CreateBusinessUnitValidator(IBusinessUnitRepository businessUnitRepository)
    {
        _businessUnitRepository = businessUnitRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.CurrentUserId)
          .NotEqual(Guid.Empty).WithMessage("Invalid {PropertyName}");

        RuleFor(command => command.Description)
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .MaximumLength(100).WithMessage("{PropertyName} must have a maximum of 100 characters")
            .MustAsync(async (businessUnitName, cancellationToken) =>
            {
                var exists = await _businessUnitRepository.ExistsByNameAsync(businessUnitName, cancellationToken);
                return !exists;
            }).WithMessage("This {PropertyName} has already been taken");
    }
}