using FluentValidation;
using MyFinance.Application.Common.CustomValidationRules;

namespace MyFinance.Application.UseCases.Users.Commands.UpdatePassword;

public sealed class UpdatePasswordValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordValidator()
    {
        RuleFor(command => command.PlainTextNewPassword)
            .MustBeAStrongPassword();

        RuleFor(command => command.PlainTextNewPassword)
            .NotEqual(command => command.PlainTextCurrentPassword)
            .WithMessage("{PropertyName} and {ComparisonProperty} must not be equal");

        RuleFor(command => command.PlainTextNewPasswordConfirmation)
            .Equal(command => command.PlainTextNewPassword)
            .WithMessage("The {PropertyName} and {ComparisonProperty} do not match");
    }
}
