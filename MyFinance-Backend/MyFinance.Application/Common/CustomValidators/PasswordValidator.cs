using FluentValidation;

namespace MyFinance.Application.Common.CustomValidationRules;

public static class PasswordValidator
{
    private static readonly Func<char, bool> IsLowerCase = c => c >= 'a' && c <= 'z';
    private static readonly Func<char, bool> IsUpperCase = c => c >= 'A' && c <= 'Z';
    private static readonly Func<char, bool> IsNumber = c => c >= '0' && c <= '9';
    private static readonly Func<char, bool> IsNonAlphaNumeric = c => !(IsLowerCase(c) || IsUpperCase(c) || IsNumber(c));

    public static IRuleBuilderOptions<T, string> MustBeAStrongPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .NotNull().WithMessage("{PropertyName} must not be null")
            .MinimumLength(16).WithMessage("{PropertyName} must have at least 16 characters")
            .Must(plainTextPassword =>
            {
                var hasTwoNonAlphaNumeric = plainTextPassword.Count(IsNonAlphaNumeric) >= 2;
                return hasTwoNonAlphaNumeric;
            }).WithMessage("{PropertyName} must have at least 2 non alphanumeric digits")
            .Must(plainTextPassword =>
            {
                var hasTwoNumbers = plainTextPassword.Count(IsNumber) >= 2;
                return hasTwoNumbers;
            }).WithMessage("{PropertyName} must have at least 2 numbers")
            .Must(plainTextPassword =>
            {
                var hasTwoUpperCaseLetters = plainTextPassword.Count(IsUpperCase) >= 2;
                return hasTwoUpperCaseLetters;
            }).WithMessage("{PropertyName} must have at least 2 upper case letters")
            .Must(plainTextPassword =>
            {
                var hasTwoLowerCaseLetters = plainTextPassword.Count(IsLowerCase) >= 2;
                return hasTwoLowerCaseLetters;
            }).WithMessage("{PropertyName} must have at least 2 lower case letters");
    }
}
