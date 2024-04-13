using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyFinance.Contracts.HealthCheck.Responses;

namespace MyFinance.Application.Mappers;

public static class HealthChecksMapper
{
    public static class DTR
    {
        public static HealthyServicesResponse Map(HealthReport healthReport)
            => new()
            {
                IsHealthy = healthReport.Status == HealthStatus.Healthy,
                Status = healthReport.Status.ToString(),
                TotalDuration = healthReport.TotalDuration.ToString(),
                Services = healthReport.Entries.Select(entry => new ServiceHealthResponse
                {
                    Name = entry.Key,
                    Status = entry.Value.Status.ToString(),
                    Duration = entry.Value.Duration.ToString(),
                    Description = entry.Value.Description ?? entry.Value.Exception?.Message,
                    ExceptionMessage = entry.Value.Exception?.Message,
                    Data = entry.Value.Data,
                    Tags = entry.Value.Tags.ToList().AsReadOnly()
                }).ToList().AsReadOnly()
            };
    }

    public static class ETR
    {
        public static UnhealthyServicesResponse Map(ProblemDetails problemDetails, HealthReport healthReport)
        {
            return new(problemDetails, healthReport)
            {
                Services = healthReport.Entries.Select(entry => new ServiceHealthResponse
                {
                    Name = entry.Key,
                    Status = entry.Value.Status.ToString(),
                    Duration = entry.Value.Duration.ToString(),
                    Description = entry.Value.Description ?? entry.Value.Exception?.Message,
                    ExceptionMessage = entry.Value.Exception?.Message,
                    Data = entry.Value.Data,
                    Tags = entry.Value.Tags.ToList().AsReadOnly()
                }).ToList().AsReadOnly()
            };
        }
    }
}
