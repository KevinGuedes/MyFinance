using FluentValidation;
using MyFinance.Application.Common.CustomValidationRules;

namespace MyFinance.Application.UseCases.Users.Commands.ResetPassword;

public sealed class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator()
    {
        RuleFor(command => command.UrlSafeResetPasswordToken)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty");

        RuleFor(command => command.PlainTextNewPassword)
            .MustBeAStrongPassword();

        RuleFor(command => command.PlainTextNewPasswordConfirmation)
            .Equal(command => command.PlainTextNewPassword)
            .WithMessage("The {PropertyName} and {ComparisonProperty} do not match");
    }
}
