namespace MyFinance.Contracts.AccountTag.Responses;

public sealed class AccountTagResponse
{
    public required Guid Id { get; init; }
    public required string Tag { get; init; }
    public required string? Description { get; init; }
    public required bool IsArchived { get; init; }
    public required string? ReasonToArchive { get; init; }
}