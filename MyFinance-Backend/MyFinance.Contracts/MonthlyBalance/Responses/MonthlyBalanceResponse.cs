namespace MyFinance.Contracts.MonthlyBalance.Responses;

public sealed class MonthlyBalanceResponse
{
    public required Guid Id { get; init; }
    public required double Income { get; init; }
    public required double Outcome { get; init; }
    public required double Balance { get; init; }
    public required int ReferenceMonth { get; init; }
    public required int ReferenceYear { get; init; }
}
