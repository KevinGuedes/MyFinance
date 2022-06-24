using FluentValidation;
using MyFinance.Application.Transfers.Commands.DeleteTransfer;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Transfers.Commands.DeleteTransferBy
{
    public sealed class DeleteTransferByIdValidator : AbstractValidator<DeleteTransferCommand>
    {
        private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;

        public DeleteTransferByIdValidator(IMonthlyBalanceRepository monthlyBalanceRepository)
        {
            _monthlyBalanceRepository = monthlyBalanceRepository;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(command => command.TransferId)
                .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");

            RuleFor(command => command.Month)
                .GreaterThan(0).WithMessage("{PropertyName} muste be greater than 0")
                .LessThanOrEqualTo(12).WithMessage("{PropertyName} muste be lower or equal to 12");

            RuleFor(command => command.Year)
                .GreaterThan(2000).WithMessage("{PropertyName} muste be greater than 2000")
                .LessThanOrEqualTo(9999).WithMessage("{PropertyName} muste be less than 9999");

            RuleFor(command => command)
                .MustAsync(async (command, cancellationToken) =>
                {
                    var exists = await 
                        _monthlyBalanceRepository.ExistsByMonthAndYearAsync(command.Month, command.Year, cancellationToken);
                    return exists;
                }).WithMessage("Business Unit with {PropertyName} doesn't exist");
        }
    }
}
