namespace MyFinance.Contracts.HealthCheck.Responses;

public sealed class HealthyServicesResponse
{
    public required bool IsHealthy { get; init; }
    public required string Status { get; init; }
    public required string TotalDuration { get; init; }
    public required IReadOnlyCollection<ServiceHealthResponse> Services { get; init; }
}
