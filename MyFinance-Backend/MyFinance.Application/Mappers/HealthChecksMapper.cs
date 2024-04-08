using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyFinance.Contracts.HealthCheck.Responses;

namespace MyFinance.Application.Mappers;

public static class HealthChecksMapper
{
    public static class DTR
    {
        public static HealthChecksReportResponse Map(HealthReport healthReport)
            => new()
            {
                IsHealthy = healthReport.Status == HealthStatus.Healthy,
                Status = healthReport.Status.ToString(),
                TotalDuration = healthReport.TotalDuration.ToString(),
                Instances = healthReport.Entries.Select(entry => new InstanceStatusResponse
                {
                    Name = entry.Key,
                    Status = entry.Value.Status.ToString(),
                    Duration = entry.Value.Duration.ToString(),
                    Description = entry.Value.Description,
                    Data = entry.Value.Data,
                    Tags = entry.Value.Tags.ToList().AsReadOnly()
                }).ToList()
            };
    }
}
