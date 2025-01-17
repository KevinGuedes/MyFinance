﻿using FluentValidation;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.AccountTags.Commands.ArchiveAccountTag;

public sealed class ArchiveAccountTagValidator : AbstractValidator<ArchiveAccountTagCommand>
{
    public ArchiveAccountTagValidator()
    {
        RuleFor(command => command.ReasonToArchive)
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Id).MustBeAValidGuid();
    }
}