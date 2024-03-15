using FluentValidation;

namespace MyFinance.Application.UseCases.BusinessUnits.Queries.GetBusinessUnits;

public class GetBusinessUnitsValidator : AbstractValidator<GetBusinessUnitsQuery>
{
    public GetBusinessUnitsValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

        RuleFor(query => query.PageSize)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");
    }
}