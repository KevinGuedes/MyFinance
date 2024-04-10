using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;

public sealed class UpdateTransferValidator : AbstractValidator<UpdateTransferCommand>
{
    public UpdateTransferValidator()
    {
        RuleFor(command => command.Id).MustBeAValidGuid();
        RuleFor(command => command.CurrentUserId).MustBeAValidGuid();
        RuleFor(command => command.AccountTagId).MustBeAValidGuid();
        RuleFor(command => command.CategoryId).MustBeAValidGuid();

        RuleFor(command => command.Value)
            .NotEqual(0).WithMessage("{PropertyName} must not be equal to 0")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

        RuleFor(command => command.RelatedTo)
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .NotNull().WithMessage("{PropertyName} must not be null")
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Description)
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .NotNull().WithMessage("{PropertyName} must not be null")
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Type)
            .IsInEnum().WithMessage("Invalid {PropertyName}");
    }
}