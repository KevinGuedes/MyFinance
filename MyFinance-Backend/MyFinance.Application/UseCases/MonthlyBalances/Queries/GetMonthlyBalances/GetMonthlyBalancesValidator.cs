using FluentValidation;

namespace MyFinance.Application.UseCases.MonthlyBalances.Queries.GetMonthlyBalances;

public sealed class GetMonthlyBalancesValidator : AbstractValidator<GetMonthlyBalancesQuery>
{
    public GetMonthlyBalancesValidator()
    {
        RuleFor(query => query.Page)
           .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

        RuleFor(query => query.PageSize)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

        RuleFor(query => query.BusinessUnitId)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");
    }
}
