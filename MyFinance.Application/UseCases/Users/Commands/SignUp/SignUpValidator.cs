using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.Users.Commands.SignUp;

public sealed class SignUpValidator : AbstractValidator<SignUpCommand>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext;

    public SignUpValidator(IMyFinanceDbContext myFinanceDbContext)
    {
        _myFinanceDbContext = myFinanceDbContext;
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
            .EmailAddress().WithMessage("{PropertyName} is not valid")
            .MustAsync(async (email, cancellationToken) =>
            {
                var exists = await _myFinanceDbContext.Users
                    .AnyAsync(user => user.Email == email, cancellationToken);

                return !exists;
            }).WithMessage("The {PropertyName} {PropertyValue} has already been taken");
    }
}