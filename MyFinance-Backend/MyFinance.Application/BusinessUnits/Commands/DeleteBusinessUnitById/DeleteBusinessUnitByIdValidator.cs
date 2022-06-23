using FluentValidation;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.BusinessUnits.Commands.DeleteBusinessUnitById
{
    public sealed class DeleteBusinessUnitByIdValidator : AbstractValidator<DeleteBusinessUnitByIdCommand>
    {
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public DeleteBusinessUnitByIdValidator(IBusinessUnitRepository businessUnitRepository)
        {
            _businessUnitRepository = businessUnitRepository;

            RuleFor(command => command.BusinessUnitId)
                .Cascade(CascadeMode.Stop)
                .NotEqual(Guid.Empty)
                .MustAsync(async (businessUnitId, cancellationToken) =>
                {
                    var exists = await _businessUnitRepository.ExistsByIdAsync(businessUnitId, cancellationToken);
                    return exists;
                }).WithMessage("Business Unit with {PropertyName} doesn't exist");
        }
    }
}
