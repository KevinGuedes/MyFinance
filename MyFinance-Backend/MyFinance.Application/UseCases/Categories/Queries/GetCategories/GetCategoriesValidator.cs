using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.Categories.Queries.GetCategories;

public sealed class GetCategoriesValidator : AbstractValidator<GetCategoriesQuery>
{
    public GetCategoriesValidator()
    {
        RuleFor(query => query.PageNumber).MustBeAValidPageNumber();
        RuleFor(query => query.PageSize).MustBeLessThan100();
    }
}