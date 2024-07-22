using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetMonthlySummary;

public sealed class GetMonthlySummaryValidator : AbstractValidator<GetMonthlySummaryQuery>
{
    public GetMonthlySummaryValidator()
    {
        RuleFor(query => query.Id).MustBeAValidGuid();

        RuleFor(query => query.Year)
            .GreaterThan(1900).WithMessage("{PropertyName} must be greater than 1900");

        RuleFor(query => query.Month)
            .InclusiveBetween(1, 12).WithMessage("{PropertyName} must be between 1 and 12");
    }
}