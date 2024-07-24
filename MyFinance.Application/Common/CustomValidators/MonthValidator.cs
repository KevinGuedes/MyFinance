using FluentValidation;

namespace MyFinance.Application.Common.CustomValidators;

public static class MonthValidator
{
    public static IRuleBuilderOptions<T, int> MustBeAValidMonth<T>(this IRuleBuilder<T, int> ruleBuilder)
        => ruleBuilder
            .InclusiveBetween(1, 12).WithMessage("{PropertyName} must be between 1 and 12");
}
