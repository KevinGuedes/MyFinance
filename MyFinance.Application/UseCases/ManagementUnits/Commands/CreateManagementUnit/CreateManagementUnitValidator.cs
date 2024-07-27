using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.CreateManagementUnit;

public sealed class CreateManagementUnitValidator : AbstractValidator<CreateManagementUnitCommand>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext;

    public CreateManagementUnitValidator(IMyFinanceDbContext myFinanceDbContext)
    {
        _myFinanceDbContext = myFinanceDbContext;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Description)
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .MaximumLength(100).WithMessage("{PropertyName} must have a maximum of 100 characters")
            .MustAsync(async (managementUnitName, cancellationToken) =>
            {
                var exists = await _myFinanceDbContext.ManagementUnits
                    .AnyAsync(mu => mu.Name == managementUnitName, cancellationToken);

                return !exists;
            }).WithMessage("The name '{PropertyValue}' has already been taken");
    }
}