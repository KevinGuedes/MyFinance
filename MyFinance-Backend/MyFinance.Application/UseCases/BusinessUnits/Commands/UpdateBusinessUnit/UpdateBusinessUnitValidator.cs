using FluentValidation;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;

public sealed class UpdateBusinessUnitValidator : AbstractValidator<UpdateBusinessUnitCommand>
{
    private readonly IBusinessUnitRepository _businessUnitRepository;

    public UpdateBusinessUnitValidator(IBusinessUnitRepository businessUnitRepository)
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

        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");

        RuleFor(command => command.Name)
           .Cascade(CascadeMode.Stop)
           .NotNull().WithMessage("{PropertyName} must not be null")
           .NotEmpty().WithMessage("{PropertyName} must not be empty")
           .Length(3, 30).WithMessage("{PropertyName} must have between 3 and 30 characters")
           .MustAsync(async (command, newBusinessUnitName, cancellationToken) =>
           {
               var existingBusinessUnit = await _businessUnitRepository.GetByNameAsync(newBusinessUnitName, cancellationToken);
               if (existingBusinessUnit is null)
                   return true;

               var isValidName = existingBusinessUnit.Id == command.Id;
               return isValidName;
           }).WithMessage("This {PropertyName} has already been taken");
    }
}
