using FluentValidation;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetTransfers;

internal class GetTransfersValidator : AbstractValidator<GetTransfersQuery>
{
    public GetTransfersValidator()
    {
        When(query => 
            query.From.HasValue && query.From.Value != default && 
            query.To.HasValue && query.To.Value != default,
            () =>
        {
            RuleFor(query => query.From)
                .LessThan(query => query.To)
                .WithMessage("End date must not be after start date");
        });

        RuleFor(query => query.BusinessUnitId)
           .NotEqual(Guid.Empty).WithMessage("Invalid {PropertyName}");

        RuleFor(query => query.BusinessUnitId)
            .NotEqual(Guid.Empty).WithMessage("Invalid {PropertyName}");

        RuleFor(query => query.AccountTagId)
            .NotEqual(Guid.Empty).WithMessage("Invalid {PropertyName}");

        RuleFor(query => query.PageNumber)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

        RuleFor(query => query.PageSize)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");
    }
}