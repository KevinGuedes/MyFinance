using FluentValidation;

namespace MyFinance.Application.Common.CustomValidators;

public static class PaginationValidator
{
    public static IRuleBuilderOptions<T, int> MustBeAValidPageNumber<T>(this IRuleBuilder<T, int> ruleBuilder)
        => ruleBuilder
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

    public static IRuleBuilderOptions<T, int> MustBeLessThan100<T>(this IRuleBuilder<T, int> ruleBuilder)
     => ruleBuilder
        .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0")
        .LessThanOrEqualTo(100).WithMessage("{PropertyName} must be 100 or less");

    public static IRuleBuilderOptions<T, int> MustBeLessThan10<T>(this IRuleBuilder<T, int> ruleBuilder)
         => ruleBuilder
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0")
            .LessThanOrEqualTo(10).WithMessage("{PropertyName} must be 10 or less");
}
