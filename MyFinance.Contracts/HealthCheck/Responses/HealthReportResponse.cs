using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MyFinance.Contracts.HealthCheck.Responses;

public sealed class HealthReportResponse(HealthReport healthReport)
{
    public bool IsHealthy { get; init; } = healthReport.Status == HealthStatus.Healthy;
    public string ApplicationStatus { get; init; } = healthReport.Status.ToString();
    public string TotalDuration { get; init; } = healthReport.TotalDuration.ToString();
    public IReadOnlyCollection<ServiceHealthResponse> Services { get; init; } = healthReport.Entries
        .Select(entry => new ServiceHealthResponse
        {
            Name = entry.Key,
            Status = entry.Value.Status.ToString(),
            Duration = entry.Value.Duration.ToString(),
            Description = entry.Value.Description ?? entry.Value.Exception?.Message,
            ExceptionMessage = entry.Value.Exception?.Message,
        })
        .ToList()
        .AsReadOnly();
}
