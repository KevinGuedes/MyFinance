using FluentValidation;

namespace MyFinance.Application.UseCases.Users.Commands.MagicSignIn;

public sealed class MagicSignInValidator : AbstractValidator<MagicSignInCommand>
{
    public MagicSignInValidator()
    {
        RuleFor(command => command.UrlSafeMagicSignInToken)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty");
    }
}
