namespace MyFinance.Contracts.Transfer.Responses;

public class PeriodBalanceResponse
{
    public required decimal Income { get; init; }
    public required decimal Outcome { get; init; }
    public decimal Balance => Income - Outcome;
}
