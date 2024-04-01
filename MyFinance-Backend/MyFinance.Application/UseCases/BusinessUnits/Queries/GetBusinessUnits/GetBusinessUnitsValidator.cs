using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.BusinessUnits.Queries.GetBusinessUnits;

public class GetBusinessUnitsValidator : AbstractValidator<GetBusinessUnitsQuery>
{
    public GetBusinessUnitsValidator()
    {
        RuleFor(query => query.PageNumber).MustBeAValidPageNumber();
        RuleFor(query => query.PageSize).MustBeLessThan10();
    }
}