using FluentValidation;
using MyFinance.Application.Abstractions.Persistence.Repositories;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;

public sealed class UpdateBusinessUnitValidator : AbstractValidator<UpdateBusinessUnitCommand>
{
    private readonly IBusinessUnitRepository _businessUnitRepository;

    public UpdateBusinessUnitValidator(IBusinessUnitRepository businessUnitRepository)
    {
        _businessUnitRepository = businessUnitRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Description)
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");

        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .MaximumLength(100).WithMessage("{PropertyName} must have a maximum of 100 characters")
            .MustAsync(async (command, newBusinessUnitName, cancellationToken) =>
            {
                var existingBusinessUnit = await _businessUnitRepository.GetByNameAsync(
                    newBusinessUnitName,
                    cancellationToken);

                if (existingBusinessUnit is null)
                    return true;

                var isValidName = existingBusinessUnit.Id == command.Id;
                return isValidName;
            }).WithMessage("The {PropertyName} {PropertyValue} has already been taken");
    }
}