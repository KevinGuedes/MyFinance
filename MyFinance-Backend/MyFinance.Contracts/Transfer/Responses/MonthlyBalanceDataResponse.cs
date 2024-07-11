namespace MyFinance.Contracts.Transfer.Responses;

public sealed class MonthlyBalanceDataResponse
{
    public required int Year { get; init; }
    public required int Month { get; init; }
    public required decimal Income { get; init; }
    public required decimal Outcome { get; init; }
    public required string Reference { get; init; }
    public decimal Balance => Income - Outcome;
}
