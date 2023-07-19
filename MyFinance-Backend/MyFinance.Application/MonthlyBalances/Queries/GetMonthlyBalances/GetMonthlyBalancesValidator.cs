using FluentValidation;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.MonthlyBalances.Queries.GetMonthlyBalances;

public sealed class GetMonthlyBalancesValidator : AbstractValidator<GetMonthlyBalancesQuery>
{
    private readonly IBusinessUnitRepository _businessUnitRepository;

    public GetMonthlyBalancesValidator(IBusinessUnitRepository businessUnitRepository)
    {
        _businessUnitRepository = businessUnitRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(query => query.Take)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

        RuleFor(query => query.Skip)
           .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be 0 or greater");

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
