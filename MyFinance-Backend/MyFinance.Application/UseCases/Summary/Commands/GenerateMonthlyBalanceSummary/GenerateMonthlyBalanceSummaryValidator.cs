using FluentValidation;

namespace MyFinance.Application.UseCases.Summary.Commands.GenerateMonthlyBalanceSummary;

public sealed class GenerateMonthlyBalanceSummaryValidator : AbstractValidator<GenerateMonthlyBalanceSummaryCommand>
{
    public GenerateMonthlyBalanceSummaryValidator()
    {
        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");
    }
}
