using FluentValidation;

namespace MyFinance.Application.MonthlyBalances.Queries.GetRecentMonthlyBalances
{
    public sealed class GetMonthlyBalancesValidator : AbstractValidator<GetMonthlyBalancesQuery>
    {
        public GetMonthlyBalancesValidator()
        {
            RuleFor(query => query.Count)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero");

            RuleFor(query => query.Skip)
               .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be zero or greater");
        }
    }
}
