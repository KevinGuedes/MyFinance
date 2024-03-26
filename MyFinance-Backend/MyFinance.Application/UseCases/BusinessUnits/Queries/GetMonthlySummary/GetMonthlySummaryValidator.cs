using FluentValidation;

namespace MyFinance.Application.UseCases.BusinessUnits.Queries.GetMonthlySummary;

public sealed class GetMonthlySummaryValidator : AbstractValidator<GetMonthlySummaryQuery>
{
    public GetMonthlySummaryValidator()
    {
        RuleFor(query => query.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");

        RuleFor(query => query.Year)
            .GreaterThan(2000).WithMessage("{PropertyName} must be greater than 2000");

        RuleFor(query => query.Month)
            .InclusiveBetween(1, 12).WithMessage("{PropertyName} must be between 1 and 12");
    }
}