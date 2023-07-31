﻿using FluentValidation;

namespace MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfers;

public sealed class RegisterTransfersValidator : AbstractValidator<RegisterTransfersCommand>
{
    public RegisterTransfersValidator()
    {
        RuleFor(command => command.Value)
            .NotEqual(0).WithMessage("{PropertyName} must not be equal to 0")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

        RuleFor(transferData => transferData.RelatedTo)
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .NotNull().WithMessage("{PropertyName} must not be null")
            .Length(3, 50).WithMessage("{PropertyName} must have between 3 and 50 characters");

        RuleFor(transferData => transferData.Description)
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .NotNull().WithMessage("{PropertyName} must not be null")
            .Length(10, 140).WithMessage("{PropertyName} must have between 10 and 140 characters");

        RuleFor(transferData => transferData.Type)
            .IsInEnum().WithMessage("Invalid {PropertyName}");

        RuleFor(command => command.BusinessUnitId)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");
    }
}