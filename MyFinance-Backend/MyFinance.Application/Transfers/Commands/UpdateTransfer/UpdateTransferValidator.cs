using FluentValidation;
using MyFinance.Domain.Enums;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Transfers.Commands.UpdateTransfer;

public sealed class UpdateTransferValidator : AbstractValidator<UpdateTransferCommand>
{
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;

    public UpdateTransferValidator(IMonthlyBalanceRepository monthlyBalanceRepository)
    {
        _monthlyBalanceRepository = monthlyBalanceRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Value)
            .NotEqual(0).WithMessage("{PropertyName} must not be equal to 0");

        When(transferData => transferData.Value > 0, () =>
        {
            RuleFor(transferData => transferData.TransferType)
                .Equal(TransferType.Profit)
                .WithMessage("Type not assignable for this Value");
        }).Otherwise(() =>
        {
            RuleFor(transferData => transferData.TransferType)
                .Equal(TransferType.Expense)
                .WithMessage("Type not assignable for this Value");
        });

        RuleFor(command => command.RelatedTo)
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .NotNull().WithMessage("{PropertyName} must not be null")
            .Length(3, 50).WithMessage("{PropertyName} must have between 3 and 50 characters");

        RuleFor(command => command.Description)
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .NotNull().WithMessage("{PropertyName} must not be null")
            .Length(10, 140).WithMessage("{PropertyName} must have between 10 and 140 characters");

        RuleFor(command => command.TransferId)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");

        RuleFor(command => command.CurrentMonthlyBalanceId)
            .Cascade(CascadeMode.Stop)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid")
            .MustAsync(async (monthlyBalanceId, cancellationToken) =>
            {
                var exists = await _monthlyBalanceRepository.ExistsByIdAsync(monthlyBalanceId, cancellationToken);
                return exists;
            }).WithMessage("Monthly Balance not found");
    }
}
