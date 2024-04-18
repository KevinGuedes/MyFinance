﻿using FluentValidation;

namespace MyFinance.Application.UseCases.Users.Commands.SignIn;

public sealed class SignInValidator : AbstractValidator<SignInCommand>
{
    public SignInValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .NotNull().WithMessage("{PropertyName} must not be null")
            .MaximumLength(256).WithMessage("{PropertyName} must not exceed 256 characters")
            .EmailAddress().WithMessage("{PropertyName} is not valid");

        RuleFor(command => command.PlainTextPassword)
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .NotNull().WithMessage("{PropertyName} must not be null");
    }
}