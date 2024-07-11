namespace MyFinance.Contracts.Transfer.Responses;

public sealed class DiscriminatedBalanceDataResponse
{
    public required IReadOnlyCollection<MonthlyBalanceDataResponse> DiscriminatedBalanceData { get; init; }
}
