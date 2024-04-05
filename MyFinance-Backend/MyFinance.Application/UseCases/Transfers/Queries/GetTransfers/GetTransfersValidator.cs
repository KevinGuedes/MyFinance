using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetTransfers;

public sealed class GetTransfersValidator : AbstractValidator<GetTransfersQuery>
{
    public GetTransfersValidator()
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

        RuleFor(query => query.PageNumber).MustBeAValidPageNumber();
        RuleFor(query => query.PageSize).MustBeLessThan100();
    }
}