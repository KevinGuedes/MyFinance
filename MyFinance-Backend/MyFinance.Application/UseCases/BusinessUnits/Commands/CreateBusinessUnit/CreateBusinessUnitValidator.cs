using FluentValidation;
using MyFinance.Application.Services.CurrentUserProvider;
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
               var currentUserId = _currentUserProvider.GetCurrentUserId();
               var exists = await _businessUnitRepository.ExistsByNameAsync(businessUnitName, currentUserId, cancellationToken);
               return !exists;
           }).WithMessage("This {PropertyName} has already been taken");
    }
}
