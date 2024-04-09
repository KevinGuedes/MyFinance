namespace MyFinance.Contracts.Transfer.Responses;

public sealed class AnnualBalanceDataResponse
{
    public required int Year { get; init; }
    public required IReadOnlyCollection<MonthlyBalanceDataResponse> MonthlyBalanceData { get; init; }
}
