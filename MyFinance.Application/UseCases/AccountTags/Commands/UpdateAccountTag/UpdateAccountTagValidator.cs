using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;

public sealed class UpdateAccountTagValidator : AbstractValidator<UpdateAccountTagCommand>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext;

    public UpdateAccountTagValidator(IMyFinanceDbContext myFinanceDbContext)
    {
        _myFinanceDbContext = myFinanceDbContext;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Description)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .MaximumLength(300)
            .WithMessage("{PropertyName} must have a maximum of 300 charactersmust have a maximum of 300 characters");

        RuleFor(command => command.Id).MustBeAValidGuid();

        RuleFor(command => command.Tag)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .Length(3, 10).WithMessage("{PropertyName} must have between 3 and 10 characters")
            .MustAsync(async (command, tag, cancellationToken) =>
            {
                var existingAccountTagId = await _myFinanceDbContext.AccountTags
                    .Where(at => at.Tag == tag)
                    .Select(at => at.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingAccountTagId == default)
                    return true;

                var isValidTag = existingAccountTagId == command.Id;
                return isValidTag;
            }).WithMessage("This {PropertyName} has already been taken");
    }
}