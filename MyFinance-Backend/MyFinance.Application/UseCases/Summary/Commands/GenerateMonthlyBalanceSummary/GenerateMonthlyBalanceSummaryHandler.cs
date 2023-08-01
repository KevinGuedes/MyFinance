using FluentResults;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.Summary.Commands.GenerateMonthlyBalanceSummary;

internal sealed class GenerateMonthlyBalanceSummaryHandler : ICommandHandler<GenerateMonthlyBalanceSummaryCommand, int>
{
    private readonly IMonthlyBalanceRepository _monthlyBalanceRepository;

    public GenerateMonthlyBalanceSummaryHandler(IMonthlyBalanceRepository monthlyBalanceRepository)
        => _monthlyBalanceRepository = monthlyBalanceRepository;

    public async Task<Result<int>> Handle(GenerateMonthlyBalanceSummaryCommand command, CancellationToken cancellationToken)
    {
        var monthlyBalance = await _monthlyBalanceRepository.GetByIdAsync(command.Id, cancellationToken);



        return 0;
    }
}
