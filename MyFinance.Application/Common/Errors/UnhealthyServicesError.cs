using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MyFinance.Application.Common.Errors;

public sealed class UnhealthyServicesError(HealthReport healthReport)
    : BaseError($"Application is {healthReport.Status}")
{
    public HealthReport HealthReport { get; } = healthReport;
}
