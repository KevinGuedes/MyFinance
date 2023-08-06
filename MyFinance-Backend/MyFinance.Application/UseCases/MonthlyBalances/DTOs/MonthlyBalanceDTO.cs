using MyFinance.Application.Common.DTO;

namespace MyFinance.Application.UseCases.MonthlyBalances.DTOs;

public sealed class MonthlyBalanceDTO : BaseDTO
{
    public required double Income { get; init; }
    public required double Outcome { get; init; }
    public required double Balance { get; init; }
    public required int ReferenceMonth { get; init; }
    public required int ReferenceYear { get; init; }
}
