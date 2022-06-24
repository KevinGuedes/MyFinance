using FluentValidation;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Transfers.Commands.UpdateTransfer
{
    public sealed class UpdateTransferValidator : AbstractValidator<UpdateTransferCommand>
    {
        private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;

        public UpdateTransferValidator(IMonthlyBalanceRepository monthlyBalanceRepository)
        {
            _monthlyBalanceRepository = monthlyBalanceRepository;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(command => command.AbsoluteValue)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

            RuleFor(command => command.RelatedTo)
                .NotEmpty().WithMessage("{PropertyName} must not be empty")
                .NotNull().WithMessage("{PropertyName} must not be null")
                .Length(2, 50).WithMessage("{PropertyName} must have between 2 and 50 characters");

            RuleFor(command => command.Description)
                .NotEmpty().WithMessage("{PropertyName} must not be empty")
                .NotNull().WithMessage("{PropertyName} must not be null")
                .Length(5, 140).WithMessage("{PropertyName} must have between 5 and 140 characters");

            RuleFor(command => command.TransferType)
                .IsInEnum().WithMessage("Invalid {PropertyName}");

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
}
