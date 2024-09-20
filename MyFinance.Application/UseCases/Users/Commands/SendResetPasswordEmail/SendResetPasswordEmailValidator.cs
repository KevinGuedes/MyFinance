using FluentValidation;

namespace MyFinance.Application.UseCases.Users.Commands.SendResetPasswordEmail;

public sealed class SendResetPasswordEmailValidator : AbstractValidator<SendResetPasswordEmailCommand>
{
    public SendResetPasswordEmailValidator()
    {
        RuleFor(command => command.Email)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .EmailAddress().WithMessage("{PropertyName} must be a valid email address");
    }
}
