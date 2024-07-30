using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Common.CustomValidators;

namespace MyFinance.Application.UseCases.ManagementUnits.Commands.UpdateManagementUnit;

public sealed class UpdateManagementUnitValidator : AbstractValidator<UpdateManagementUnitCommand>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext;

    public UpdateManagementUnitValidator(IMyFinanceDbContext myFinanceDbContext)
    {
        _myFinanceDbContext = myFinanceDbContext;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Description)
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Id).MustBeAValidGuid();

        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .MaximumLength(100).WithMessage("{PropertyName} must have a maximum of 100 characters")
            .MustAsync(async (command, managementUnitName, cancellationToken) =>
            {
                var existingManagementUnitId = await _myFinanceDbContext.ManagementUnits
                    .Where(mu => mu.Name == managementUnitName)
                    .Select(mu => mu.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingManagementUnitId == default)
                    return true;

                var isValid = existingManagementUnitId == command.Id;
                return isValid;
            }).WithMessage("The {PropertyName} {PropertyValue} has already been taken");
    }
}