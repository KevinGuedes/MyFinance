using FluentValidation;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.BusinessUnits.Commands.CreateBusinessUnit
{
    public class CreateBusinessUnitCommandValidator : AbstractValidator<CreateBusinessUnitCommand>
    {
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public CreateBusinessUnitCommandValidator(IBusinessUnitRepository businessUnitRepository)
        {
            _businessUnitRepository = businessUnitRepository;

            RuleFor(command => command.Name)
               .Cascade(CascadeMode.Stop)
               .NotNull().WithMessage("{PropertyName} can not be null")
               .NotEmpty().WithMessage("{PropertyName} can not be empty")
               .Length(2, 50).WithMessage("{PropertyName} must have between 2 and 50 characters")
               .MustAsync(async (id, cancellationToken) =>
               {
                   var exists = await _businessUnitRepository.ExistsByNameAsync(id, cancellationToken);
                   return !exists;
               }).WithMessage("This {PropertyName} has already been taken");
        }
    }
}
