using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetDiscriminatedBalanceData;

public sealed class GetDiscriminatedBalanceDataValidator : AbstractValidator<GetDiscriminatedBalanceDataQuery>
{
    public GetDiscriminatedBalanceDataValidator()
    {
        RuleFor(query => query.ManagementUnitId).MustBeAValidGuid();

        RuleFor(query => query.PastMonths)
            .InclusiveBetween(1, 12)
            .WithMessage("{PropertyName} must be between 1 and 12");
    }
}
