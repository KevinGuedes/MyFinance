using FluentValidation;

namespace MyFinance.Application.Common.CustomValidators;

public static class GuidValidator
{
    public static IRuleBuilderOptions<T, Guid> MustBeAValidGuid<T>(this IRuleBuilder<T, Guid> ruleBuilder)
        => ruleBuilder
            .NotEqual(Guid.Empty).WithMessage("Invalid {PropertyName}");
}
