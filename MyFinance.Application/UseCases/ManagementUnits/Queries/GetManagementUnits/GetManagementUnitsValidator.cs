using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetManagementUnits;

public sealed class GetManagementUnitsValidator : AbstractValidator<GetManagementUnitsQuery>
{
    public GetManagementUnitsValidator()
    {
        RuleFor(query => query.PageNumber).MustBeAValidPageNumber();
        RuleFor(query => query.PageSize).MustBeLessThan10();
    }
}