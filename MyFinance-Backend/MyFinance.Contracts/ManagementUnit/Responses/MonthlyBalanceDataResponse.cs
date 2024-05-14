namespace MyFinance.Contracts.ManagementUnit.Responses;

public sealed class MonthlyBalanceDataResponse
{
    public required int Month { get; init; }
    public required decimal Income { get; init; }
    public required decimal Outcome { get; init; }
    public decimal Balance => Income - Outcome;
}
