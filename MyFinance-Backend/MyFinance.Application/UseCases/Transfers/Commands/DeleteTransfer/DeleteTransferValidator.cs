using FluentValidation;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.Transfers.Commands.DeleteTransfer;

public sealed class DeleteTransferByIdValidator : AbstractValidator<DeleteTransferCommand>
{
    private readonly ITransferRepository _transferRepository;
    public DeleteTransferByIdValidator(ITransferRepository transferRepository)
    {
        _transferRepository = transferRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Id)
            .Cascade(CascadeMode.Stop)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid")
            .MustAsync(async (transferId, cancellationToken) =>
            {
                var exists = await _transferRepository.ExistsByIdAsync(transferId, cancellationToken);
                return exists;
            }).WithMessage("Transfer not found");
    }
}
