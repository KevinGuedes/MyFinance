using FluentValidation;
using MyFinance.Application.UseCases.Summary.Queries.GetMonthlyBalanceSummary;

namespace MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;

public sealed class GetBusinessUnitSummaryValidator : AbstractValidator<GetMonthlyBalanceSummaryQuery>
{
    public GetBusinessUnitSummaryValidator()
    {
        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");
    }
}
