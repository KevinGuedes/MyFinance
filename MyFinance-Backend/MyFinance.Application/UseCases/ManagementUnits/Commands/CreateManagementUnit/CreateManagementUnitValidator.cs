using FluentValidation;
using MyFinance.Application.Abstractions.Persistence.Repositories;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.CreateManagementUnit;

public sealed class CreateManagementUnitValidator : AbstractValidator<CreateManagementUnitCommand>
{
    private readonly IManagementUnitRepository _managementUnitRepository;

    public CreateManagementUnitValidator(IManagementUnitRepository managementUnitRepository)
    {
        _managementUnitRepository = managementUnitRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Description)
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .MaximumLength(100).WithMessage("{PropertyName} must have a maximum of 100 characters")
            .MustAsync(async (managementUnitName, cancellationToken) =>
            {
                var exists = await _managementUnitRepository.ExistsByNameAsync(managementUnitName, cancellationToken);
                return !exists;
            }).WithMessage("This {PropertyName} has already been taken");
    }
}