namespace MyFinance.Contracts.AccountTag;

public sealed class UpdateAccountTagRequest
{
    public required Guid Id { get; init; }
    public required string Tag { get; init; } = null!;
    public required string? Description { get; init; }
}