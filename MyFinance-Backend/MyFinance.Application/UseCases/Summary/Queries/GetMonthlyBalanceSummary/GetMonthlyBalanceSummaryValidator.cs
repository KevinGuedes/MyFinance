using FluentValidation;

namespace MyFinance.Application.UseCases.Summary.Queries.GetMonthlyBalanceSummary;

public sealed class GetMonthlyBalanceSummaryValidator : AbstractValidator<GetMonthlyBalanceSummaryQuery>
{
    public GetMonthlyBalanceSummaryValidator()
    {
        RuleFor(query => query.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");
    }
}
