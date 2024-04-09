namespace MyFinance.Contracts.Transfer.Requests;

public sealed class GetAnnualBalanceDataRequest
{
    public required Guid BusinessUnitId { get; init; }
    public required int Year { get; init; }
}
