using FluentValidation;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Common.CustomValidationRules;

namespace MyFinance.Application.UseCases.Users.Commands.RegisterUser;

public sealed class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .NotNull().WithMessage("{PropertyName} must not be null")
            .MaximumLength(256).WithMessage("{PropertyName} must not exceed 256 characters");

        RuleFor(command => command.PlainTextPassword)
            .MustBeAStrongPassword();

        RuleFor(command => command.PlainTextPasswordConfirmation)
            .Equal(command => command.PlainTextPassword)
            .WithMessage("The {PropertyName} and {ComparisonProperty} do not match");

        RuleFor(command => command.Email)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .MaximumLength(256).WithMessage("{PropertyName} must not exceed 256 characters")
            .MustAsync(async (email, cancellationToken) =>
            {
                var exists = await _userRepository.ExistsByEmailAsync(email, cancellationToken);
                return !exists;
            }).WithMessage("The {PropertyName} {PropertyValue} has already been taken");
    }
}