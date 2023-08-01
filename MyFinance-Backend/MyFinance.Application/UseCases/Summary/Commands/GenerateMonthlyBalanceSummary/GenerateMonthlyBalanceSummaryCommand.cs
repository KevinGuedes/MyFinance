using MyFinance.Application.Common.RequestHandling.Commands;

namespace MyFinance.Application.UseCases.Summary.Commands.GenerateMonthlyBalanceSummary;

public sealed record GenerateMonthlyBalanceSummaryCommand(Guid Id) : ICommand<int>
{
}
