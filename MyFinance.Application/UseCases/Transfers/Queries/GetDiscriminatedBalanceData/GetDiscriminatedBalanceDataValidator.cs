using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetDiscriminatedBalanceData;

public sealed class GetDiscriminatedBalanceDataValidator : AbstractValidator<GetDiscriminatedBalanceDataQuery>
{
    public GetDiscriminatedBalanceDataValidator()
    {
        RuleFor(query => query.ManagementUnitId).MustBeAValidGuid();

        RuleFor(query => query.PastMonths)
            .GreaterThan(1).WithMessage("{PropertyName} must be greater than 1")
            .LessThanOrEqualTo(12).WithMessage("{PropertyName} must be less than or equal to 12");
    }
}
