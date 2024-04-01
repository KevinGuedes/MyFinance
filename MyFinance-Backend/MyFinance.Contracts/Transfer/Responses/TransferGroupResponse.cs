namespace MyFinance.Contracts.Transfer.Responses;

public sealed class TransferGroupResponse
{
    public required DateOnly Date { get; init; }
    public required IReadOnlyCollection<TransferResponse> Transfers { get; init; }
    public required decimal Income { get; init; }
    public required decimal Outcome { get; init; }
    public decimal Balance => Income - Outcome;
}
