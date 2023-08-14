using FluentValidation;

namespace MyFinance.Application.UseCases.AccountTags.Queries.GetAccountTags;

public sealed class GetAccountTagsValidator : AbstractValidator<GetAccountTagsQuery>
{
    public GetAccountTagsValidator()
    {
        RuleFor(query => query.Page)
           .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

        RuleFor(query => query.PageSize)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");
    }
}
