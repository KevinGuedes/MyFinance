using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetAnnualBalanceData;

public sealed class GetAnnualBalanceDataValidator : AbstractValidator<GetDiscriminatedAnnualBalanceDataQuery>
{
    public GetAnnualBalanceDataValidator()
    {
        RuleFor(query => query.BusinessUnitId).MustBeAValidGuid();

        RuleFor(query => query.Year)
            .GreaterThan(1900).WithMessage("Invalid {PropertyName}");
    }
}
