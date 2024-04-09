﻿using FluentValidation;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;

public sealed class CreateAccountTagValidator : AbstractValidator<CreateAccountTagCommand>
{
    private readonly IAccountTagRepository _accountTagRepository;

    public CreateAccountTagValidator(IAccountTagRepository accountTagRepository)
    {
        _accountTagRepository = accountTagRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.CurrentUserId).MustBeAValidGuid();

        RuleFor(command => command.Description)
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Tag)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .Length(2, 10).WithMessage("{PropertyName} must have between 2 and 10 characters")
            .MustAsync(async (tag, cancellationToken) =>
            {
                var exists = await _accountTagRepository.ExistsByTagAsync(tag, cancellationToken);
                return !exists;
            }).WithMessage("This {PropertyName} has already been taken");
    }
}