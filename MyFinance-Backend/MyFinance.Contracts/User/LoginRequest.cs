namespace MyFinance.Contracts.User;

public sealed record LoginRequest(string Email, string PlainTextPassword);
