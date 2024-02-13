using FluentValidation;
using MyFinance.Application.Services.CurrentUserProvider;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.UpdateBusinessUnit;

public sealed class UpdateBusinessUnitValidator : AbstractValidator<UpdateBusinessUnitCommand>
{
    private readonly IBusinessUnitRepository _businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider;

    public UpdateBusinessUnitValidator(IBusinessUnitRepository businessUnitRepository, ICurrentUserProvider currentUserProvider)
    {
        _businessUnitRepository = businessUnitRepository;
        _currentUserProvider = currentUserProvider;
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
               var currentUserId = _currentUserProvider.GetCurrentUserId();

               var existingBusinessUnit = await _businessUnitRepository.GetByNameAsync(
                   newBusinessUnitName,
                   currentUserId,
                   cancellationToken);

               if (existingBusinessUnit is null)
                   return true;

               var isValidName = existingBusinessUnit.Id == command.Id;
               return isValidName;
           }).WithMessage("The {PropertyName} {PropertyValue} has already been taken");
    }
}
