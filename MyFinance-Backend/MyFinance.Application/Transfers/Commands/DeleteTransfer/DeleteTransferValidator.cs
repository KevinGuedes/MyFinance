using FluentValidation;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Transfers.Commands.DeleteTransfer
{
    public sealed class DeleteTransferByIdValidator : AbstractValidator<DeleteTransferCommand>
    {
        private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;
        private readonly IBusinessUnitRepository _businessUnitRepository;

        public DeleteTransferByIdValidator(
            IMonthlyBalanceRepository monthlyBalanceRepository,
            IBusinessUnitRepository businessUnitRepository)
        {
            _monthlyBalanceRepository = monthlyBalanceRepository;
            _businessUnitRepository = businessUnitRepository;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(command => command.TransferId)
                .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");

            RuleFor(command => command.MonthlyBalanceId)
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
