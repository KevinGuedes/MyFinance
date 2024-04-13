using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyFinance.Contracts.Common;

namespace MyFinance.Contracts.HealthCheck.Responses;

public sealed class UnhealthyServicesResponse(ProblemDetails problemDetails, HealthReport healthReport) 
    : ProblemResponse(problemDetails)
{
    public bool IsHealthy { get; init; } = healthReport.Status == HealthStatus.Healthy;
    public string ServicesStatus { get; init; } = healthReport.Status.ToString();
    public string TotalDuration { get; init; } = healthReport.TotalDuration.ToString();
    public required IReadOnlyCollection<ServiceHealthResponse> Services { get; init; }
}
