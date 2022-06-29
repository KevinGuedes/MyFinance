using FluentValidation;
using MyFinance.Domain.Enums;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Transfers.Commands.RegisterTransfers
{
    public sealed class RegisterTransfersValidator : AbstractValidator<RegisterTransfersCommand>
    {
        private readonly IBusinessUnitRepository _businessUnitRepository;
        public RegisterTransfersValidator(IBusinessUnitRepository businessUnitRepository)
        {
            _businessUnitRepository = businessUnitRepository;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleForEach(command => command.Transfers).SetValidator(new TransferDataValidator());

            RuleFor(command => command.BusinessUnitId)
                .Cascade(CascadeMode.Stop)
                .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid")
                .MustAsync(async (businessUnitId, cancellationToken) =>
                {
                    var exists = await _businessUnitRepository.ExistsByIdAsync(businessUnitId, cancellationToken);
                    return exists;
                }).WithMessage("Business Unit not found");
        }
    }

    public sealed class TransferDataValidator : AbstractValidator<TransferData>
    {
        public TransferDataValidator()
        {
            RuleFor(command => command.Value)
              .NotEqual(0).WithMessage("{PropertyName} must not be equal to 0");

            When(transferData => transferData.Value > 0, () =>
            {
                RuleFor(transferData => transferData.Type)
                    .Equal(TransferType.Profit)
                    .WithMessage("Type not assignable for this Value");
            }).Otherwise(() =>
            {
                RuleFor(transferData => transferData.Type)
                    .Equal(TransferType.Expense)
                    .WithMessage("Type not assignable for this Value");
            });

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
        }
    }
}
