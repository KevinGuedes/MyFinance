using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext;

    public UpdateCategoryValidator(IMyFinanceDbContext myFinanceDbContext)
    {
        _myFinanceDbContext = myFinanceDbContext;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Id).MustBeAValidGuid();

        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .Length(3, 50).WithMessage("{PropertyName} must have between 3 and 50 characters")
            .MustAsync(async (command, categoryName, cancellationToken) =>
            {
                var existingCategoryId = await _myFinanceDbContext.Categories
                   .Where(mu => mu.Name == categoryName)
                   .Select(mu => mu.Id)
                   .FirstOrDefaultAsync(cancellationToken);

                if (existingCategoryId == default)
                    return true;

                var isValid = existingCategoryId == command.Id;
                return isValid;
            }).WithMessage("This {PropertyName} has already been taken");
    }
}