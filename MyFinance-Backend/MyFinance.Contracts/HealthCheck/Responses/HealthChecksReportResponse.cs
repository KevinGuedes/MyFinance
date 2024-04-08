namespace MyFinance.Contracts.HealthCheck.Responses;

public sealed class HealthChecksReportResponse
{
    public required bool IsHealthy { get; init; }
    public required string Status { get; init; }
    public required string TotalDuration { get; init; }
    public required IReadOnlyCollection<InstanceStatusResponse> Instances { get; init; }
}
