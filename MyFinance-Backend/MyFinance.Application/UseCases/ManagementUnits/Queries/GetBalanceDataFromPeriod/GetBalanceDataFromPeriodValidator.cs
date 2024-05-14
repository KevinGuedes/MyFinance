using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.ManagementUnits.Queries.GetBalanceDataFromPeriod;

public sealed class GetBalanceDataFromPeriodValidator : AbstractValidator<GetBalanceDataFromPeriodQuery>
{
    public GetBalanceDataFromPeriodValidator()
    {
        RuleFor(query => query.ManagementUnitId).MustBeAValidGuid();

        When(query =>
            query.StartDate.HasValue && query.StartDate.Value != default &&
            query.EndDate.HasValue && query.EndDate.Value != default,
            () =>
            {
                RuleFor(query => query.EndDate)
                    .GreaterThan(query => query.StartDate)
                    .WithMessage("End date must not be after start date");
            });
    }
}