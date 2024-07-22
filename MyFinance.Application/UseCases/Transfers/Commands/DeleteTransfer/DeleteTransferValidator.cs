using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;

public sealed class DeleteTransferByIdValidator : AbstractValidator<DeleteTransferCommand>
{
    public DeleteTransferByIdValidator()
    {
        RuleFor(command => command.Id).MustBeAValidGuid();
    }
}