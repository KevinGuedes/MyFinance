using FluentValidation;

namespace MyFinance.Application.UseCases.Categories.Queries.GetCategories;

public sealed class GetCategoriesValidator : AbstractValidator<GetCategoriesQuery>
{
    public GetCategoriesValidator()
    {
        RuleFor(query => query.PageNumber)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

        RuleFor(query => query.PageSize)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");
    }
}