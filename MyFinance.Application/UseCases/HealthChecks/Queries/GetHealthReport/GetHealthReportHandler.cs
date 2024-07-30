using FluentResults;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Common.Errors;
using MyFinance.Contracts.HealthCheck.Responses;

namespace MyFinance.Application.UseCases.HealthChecks.Queries.GetHealthReport;

internal sealed class GetHealthReportHandler(HealthCheckService healthCheckService) :
    IQueryHandler<GetHealthReportQuery, HealthReportResponse>
{
    private readonly HealthCheckService _healthCheckService = healthCheckService;

    public async Task<Result<HealthReportResponse>> Handle(GetHealthReportQuery query, CancellationToken cancellationToken)
    {
        var healthReport = await _healthCheckService.CheckHealthAsync(cancellationToken);

        if (healthReport.Status == HealthStatus.Unhealthy)
            return Result.Fail(new UnhealthyApplicationError(healthReport));

        return Result.Ok(new HealthReportResponse(healthReport));
    }
}
