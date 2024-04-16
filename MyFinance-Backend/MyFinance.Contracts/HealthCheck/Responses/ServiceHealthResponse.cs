namespace MyFinance.Contracts.HealthCheck.Responses;

public sealed class ServiceHealthResponse
{
    public required string Name { get; init; }
    public required string Status { get; init; }
    public required string Duration { get; init; }
    public string? Description { get; init; }
    public string? ExceptionMessage { get; init; }
}
