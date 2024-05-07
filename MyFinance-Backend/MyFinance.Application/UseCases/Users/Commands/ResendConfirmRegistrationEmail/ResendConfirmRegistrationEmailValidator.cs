using FluentValidation;

namespace MyFinance.Application.UseCases.Users.Commands.ResendConfirmRegistrationEmail;

public sealed class ResendConfirmRegistrationEmailValidator : AbstractValidator<ResendConfirmRegistrationEmailCommand>
{
    public ResendConfirmRegistrationEmailValidator()
    {
        RuleFor(command => command.Email)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .EmailAddress().WithMessage("{PropertyName} must be a valid email address");
    }
}
