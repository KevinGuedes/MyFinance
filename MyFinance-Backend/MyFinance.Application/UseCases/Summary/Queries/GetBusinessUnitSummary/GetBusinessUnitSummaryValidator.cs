using FluentValidation;

namespace MyFinance.Application.UseCases.Summary.Queries.GetBusinessUnitSummary;

public sealed class GetBusinessUnitSummaryValidator : AbstractValidator<GetBusinessUnitSummaryQuery>
{
    public GetBusinessUnitSummaryValidator()
    {
        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");
    }
}
