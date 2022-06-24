using FluentValidation;

namespace MyFinance.Application.Transfers.Commands.UpdateTransfer
{
    public sealed class UpdateTransferValidator : AbstractValidator<UpdateTransferCommand>
    {
        public UpdateTransferValidator()
        {
            RuleFor(transferData => transferData.AbsoluteValue)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

            RuleFor(transferData => transferData.RelatedTo)
                .NotEmpty().WithMessage("{PropertyName} must not be empty")
                .NotNull().WithMessage("{PropertyName} must not be null")
                .Length(2, 50).WithMessage("{PropertyName} must have between 2 and 50 characters");

            RuleFor(transferData => transferData.Description)
                .NotEmpty().WithMessage("{PropertyName} must not be empty")
                .NotNull().WithMessage("{PropertyName} must not be null")
                .Length(5, 140).WithMessage("{PropertyName} must have between 5 and 140 characters");

            RuleFor(transferData => transferData.Type)
                .IsInEnum().WithMessage("Invalid {PropertyName}");
        }
    }
}
