using FluentValidation;

namespace MyFinance.Application.UseCases.Users.Commands.ConfirmRegistration;

public sealed class ConfirmRegistrationValidator : AbstractValidator<ConfirmRegistrationCommand>
{
    public ConfirmRegistrationValidator()
    {
        RuleFor(command => command.UrlSafeConfirmRegistrationToken)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty");
    }
}
