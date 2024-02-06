using FluentValidation;

namespace MyFinance.Application.UseCases.Users.Commands;
public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .NotNull().WithMessage("{PropertyName} must not be null")
            .MaximumLength(256).WithMessage("{PropertyName} must not exceed 256 characters");

        RuleFor(command => command.Email)
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .NotNull().WithMessage("{PropertyName} must not be null")
            .MaximumLength(256).WithMessage("{PropertyName} must not exceed 256 characters")
            .EmailAddress().WithMessage("{PropertyName} is not valid");

        RuleFor(command => command.PlainTextPassword)
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .NotNull().WithMessage("{PropertyName} must not be null")
            .MinimumLength(16).WithMessage("{PropertyName} must have at least 16 characters");
    }
}
