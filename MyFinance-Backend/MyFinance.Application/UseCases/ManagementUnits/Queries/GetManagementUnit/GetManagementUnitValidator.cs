using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetManagementUnit;

public class GetManagementUnitValidator : AbstractValidator<GetManagementUnitQuery>
{
    public GetManagementUnitValidator()
    {
        RuleFor(query => query.Id).MustBeAValidGuid();
    }
}