using FluentValidation;

namespace MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;

public sealed class GetBusinessUnitSummaryValidator : AbstractValidator<GetBusinessUnitSummaryQuery>
{
    public GetBusinessUnitSummaryValidator()
    {
        RuleFor(query => query.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");

        RuleFor(query => query.Year)
            .InclusiveBetween(2000, 9999).WithMessage("Year must be between 2024 and 9999");
    }
}