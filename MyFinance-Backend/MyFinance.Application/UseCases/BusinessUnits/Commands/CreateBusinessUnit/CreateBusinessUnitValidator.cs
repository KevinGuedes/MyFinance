using FluentValidation;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.BusinessUnits.Commands.CreateBusinessUnit;

public sealed class CreateBusinessUnitValidator : AbstractValidator<CreateBusinessUnitCommand>
{
    private readonly IBusinessUnitRepository _businessUnitRepository;
    private readonly ICurrentUserProvider _currentUserProvider;

    public CreateBusinessUnitValidator(IBusinessUnitRepository businessUnitRepository, ICurrentUserProvider currentUserProvider)
    {
        _businessUnitRepository = businessUnitRepository;
        _currentUserProvider = currentUserProvider;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Description)
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Name)
           .Cascade(CascadeMode.Stop)
           .NotNull().WithMessage("{PropertyName} must not be null")
           .NotEmpty().WithMessage("{PropertyName} must not be empty")
           .MaximumLength(100).WithMessage("{PropertyName} must have a maximum of 100 characters")
           .MustAsync(async (businessUnitName, cancellationToken) =>
           {
               var currentUserId = _currentUserProvider.GetCurrentUserId();
               var exists = await _businessUnitRepository.ExistsByNameAsync(businessUnitName, currentUserId, cancellationToken);
               return !exists;
           }).WithMessage("This {PropertyName} has already been taken");
    }
}
