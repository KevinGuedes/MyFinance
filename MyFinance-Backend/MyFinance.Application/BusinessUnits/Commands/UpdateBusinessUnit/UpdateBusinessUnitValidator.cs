using FluentValidation;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.BusinessUnits.Commands.UpdateBusinessUnit
{
    public sealed class UpdateBusinessUnitValidator : AbstractValidator<UpdateBusinessUnitCommand>
    {
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public UpdateBusinessUnitValidator(IBusinessUnitRepository businessUnitRepository)
        {
            _businessUnitRepository = businessUnitRepository;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(command => command.Name)
                .NotNull().WithMessage("{PropertyName} must not be null")
                .NotEmpty().WithMessage("{PropertyName} must not be empty")
                .Length(3, 50).WithMessage("{PropertyName} must have between 3 and 50 characters");

            RuleFor(command => command.BusinessUnitId)
                .Cascade(CascadeMode.Stop)
                .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid")
                .MustAsync(async (businessUnitId, cancellationToken) =>
                {
                    var exists = await _businessUnitRepository.ExistsByIdAsync(businessUnitId, cancellationToken);
                    return exists;
                }).WithMessage("Business Unit not found");
        }
    }
}
