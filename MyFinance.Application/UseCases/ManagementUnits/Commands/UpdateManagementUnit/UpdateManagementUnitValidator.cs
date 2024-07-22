using FluentValidation;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.UpdateManagementUnit;

public sealed class UpdateManagementUnitValidator : AbstractValidator<UpdateManagementUnitCommand>
{
    private readonly IManagementUnitRepository _managementUnitRepository;

    public UpdateManagementUnitValidator(IManagementUnitRepository managementUnitRepository)
    {
        _managementUnitRepository = managementUnitRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Description)
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Id).MustBeAValidGuid();

        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .MaximumLength(100).WithMessage("{PropertyName} must have a maximum of 100 characters")
            .MustAsync(async (command, newManagementUnitName, cancellationToken) =>
            {
                var existingManagementUnit = await _managementUnitRepository.GetByNameAsync(
                    newManagementUnitName,
                    cancellationToken);

                if (existingManagementUnit is null)
                    return true;

                var isValid = existingManagementUnit.Id == command.Id;
                return isValid;
            }).WithMessage("The {PropertyName} {PropertyValue} has already been taken");
    }
}