using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.Transfers.Queries.GetTransferGroups;

public sealed class GetTransferGroupsValidator : AbstractValidator<GetTransferGroupsQuery>
{
    public GetTransferGroupsValidator()
    {
        RuleFor(query => query.ManagementUnitId).MustBeAValidGuid();
        RuleFor(query => query.PageNumber).MustBeAValidPageNumber();
        RuleFor(query => query.PageSize).MustBeLessThan100();
        RuleFor(query => query.Month).MustBeAValidMonth();
    }
}