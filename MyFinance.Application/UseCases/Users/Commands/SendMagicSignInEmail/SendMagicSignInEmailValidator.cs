using FluentValidation;

namespace MyFinance.Application.UseCases.Users.Commands.SendMagicSignInEmail;

public sealed class SendMagicSignInEmailValidator : AbstractValidator<SendMagicSignInEmailCommand>
{
    public SendMagicSignInEmailValidator()
    {
        RuleFor(command => command.Email)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .EmailAddress().WithMessage("{PropertyName} must be a valid email address");
    }
}
