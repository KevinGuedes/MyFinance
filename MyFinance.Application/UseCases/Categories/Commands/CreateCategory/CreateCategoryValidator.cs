using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;

namespace MyFinance.Application.UseCases.Categories.Commands.CreateCategory;

public sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext;

    public CreateCategoryValidator(IMyFinanceDbContext myFinanceDbContext)
    {
        _myFinanceDbContext = myFinanceDbContext;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .Length(3, 50).WithMessage("{PropertyName} must have between 3 and 50 characters")
            .MustAsync(async (categoryName, cancellationToken) =>
            {
                var exists = await _myFinanceDbContext.Categories
                    .AnyAsync(category => category.Name == categoryName, cancellationToken);

                return !exists;
            }).WithMessage("The name '{PropertyValue}' has already been taken");
    }
}