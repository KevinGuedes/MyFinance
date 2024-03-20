namespace MyFinance.Contracts.AccountTag.Requests;

public sealed class CreateAccountTagRequest
{
    public required string Tag { get; init; } = null!;
    public required string? Description { get; init; }
}