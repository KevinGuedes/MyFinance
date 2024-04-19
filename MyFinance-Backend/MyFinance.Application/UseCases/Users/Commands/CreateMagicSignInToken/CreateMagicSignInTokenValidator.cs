using FluentValidation;

namespace MyFinance.Application.UseCases.Users.Commands.CreateMagicSignInToken;

public sealed class CreateMagicSignInTokenValidator : AbstractValidator<CreateMagicSignInTokenCommand>
{
    public CreateMagicSignInTokenValidator()
    {
        RuleFor(command => command.Email)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .EmailAddress().WithMessage("{PropertyName} must be a valid email address");
    }
}
