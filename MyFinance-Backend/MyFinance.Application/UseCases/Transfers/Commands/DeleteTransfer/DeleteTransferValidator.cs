using FluentValidation;

namespace MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;

public sealed class DeleteTransferByIdValidator : AbstractValidator<DeleteTransferCommand>
{
    public DeleteTransferByIdValidator()
    {
        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");
    }
}
