using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetDiscriminatedAnnualBalanceData;

public sealed class GetDiscriminatedAnnualBalanceDataValidator : AbstractValidator<GetDiscriminatedAnnualBalanceDataQuery>
{
    public GetDiscriminatedAnnualBalanceDataValidator()
    {
        RuleFor(query => query.BusinessUnitId).MustBeAValidGuid();

        RuleFor(query => query.Year)
            .GreaterThan(1900).WithMessage("Invalid {PropertyName}");
    }
}
