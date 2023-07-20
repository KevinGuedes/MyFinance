﻿using FluentValidation;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;

public sealed class UpdateTransferValidator : AbstractValidator<UpdateTransferCommand>
{
    private readonly ITransferRepository _transferRepository;

    public UpdateTransferValidator(ITransferRepository transferRepository)
    {
        _transferRepository = transferRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Value)
            .NotEqual(0).WithMessage("{PropertyName} must not be equal to 0")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");

        RuleFor(command => command.RelatedTo)
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .NotNull().WithMessage("{PropertyName} must not be null")
            .Length(3, 50).WithMessage("{PropertyName} must have between 3 and 50 characters");

        RuleFor(command => command.Description)
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .NotNull().WithMessage("{PropertyName} must not be null")
            .Length(10, 140).WithMessage("{PropertyName} must have between 10 and 140 characters");

        RuleFor(transferData => transferData.Type)
            .IsInEnum().WithMessage("Invalid {PropertyName}");

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
