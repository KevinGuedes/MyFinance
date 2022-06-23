using FluentValidation;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.BusinessUnits.Commands.RemoveBusinessUnitById
{
    public sealed class RemoveBusinessUnitByIdValidator : AbstractValidator<RemoveBusinessUnitByIdCommand>
    {
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public RemoveBusinessUnitByIdValidator(IBusinessUnitRepository businessUnitRepository)
        {
            _businessUnitRepository = businessUnitRepository;

            RuleFor(command => command.BusinessUnitId)
                .MustAsync(async (businessUnitId, cancellationToken) =>
                {
                    var exists = await _businessUnitRepository.ExistsByIdAsync(businessUnitId, cancellationToken);
                    return exists;
                }).WithMessage("Business Unit with {PropertyName} doesn't exist");
        }
    }
}
