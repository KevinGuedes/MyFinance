using FluentValidation;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit
{
    public sealed class CreateBusinessUnitValidator : AbstractValidator<CreateBusinessUnitCommand>
    {
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public CreateBusinessUnitValidator(IBusinessUnitRepository businessUnitRepository)
        {
            _businessUnitRepository = businessUnitRepository;

            RuleFor(command => command.Name)
               .Cascade(CascadeMode.Stop)
               .NotNull().WithMessage("{PropertyName} must not be null")
               .NotEmpty().WithMessage("{PropertyName} must not be empty")
               .Length(2, 50).WithMessage("{PropertyName} must have between 2 and 50 characters")
               .MustAsync(async (businessUnitName, cancellationToken) =>
               {
                   var exists = await _businessUnitRepository.ExistsByNameAsync(businessUnitName, cancellationToken);
                   return !exists;
               }).WithMessage("This {PropertyName} has already been taken");
        }
    }
}
