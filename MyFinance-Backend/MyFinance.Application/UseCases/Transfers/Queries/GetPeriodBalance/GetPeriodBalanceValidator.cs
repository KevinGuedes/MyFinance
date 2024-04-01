using FluentValidation;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetPeriodBalance;

public sealed class GetPeriodBalanceValidator : AbstractValidator<GetPeriodBalanceQuery>
{
    public GetPeriodBalanceValidator()
    {
        When(query =>
            query.StartDate.HasValue && query.StartDate.Value != default &&
            query.EndDate.HasValue && query.EndDate.Value != default,
            () =>
            {
                RuleFor(query => query.EndDate)
                    .GreaterThan(query => query.StartDate)
                    .WithMessage("End date must not be after start date");
            });

        RuleFor(query => query.BusinessUnitId)
            .NotEqual(Guid.Empty).WithMessage("Invalid {PropertyName}");
    }
}