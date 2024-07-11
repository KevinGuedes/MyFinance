namespace MyFinance.Contracts.User.Responses;

public sealed class UserResponse
{
    public required string Name { get; init; }
    public required bool ShouldUpdatePassword { get; init; }
}
