using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.AccountTags.Queries.GetAccountTags;

public sealed class GetAccountTagsValidator : AbstractValidator<GetAccountTagsQuery>
{
    public GetAccountTagsValidator()
    {
        RuleFor(query => query.ManagementUnitId).MustBeAValidGuid();
        RuleFor(query => query.PageNumber).MustBeAValidPageNumber();
        RuleFor(query => query.PageSize).MustBeLessThan100();
    }
}