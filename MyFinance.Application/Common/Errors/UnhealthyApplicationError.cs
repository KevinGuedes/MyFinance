using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MyFinance.Application.Common.Errors;

public sealed class UnhealthyApplicationError(HealthReport healthReport)
    : BaseError($"Application is {healthReport.Status}")
{
    public HealthReport HealthReport { get; init; } = healthReport;
}
