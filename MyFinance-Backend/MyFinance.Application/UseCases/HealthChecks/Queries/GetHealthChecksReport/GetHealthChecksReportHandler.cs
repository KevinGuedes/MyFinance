using FluentResults;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.HealthCheck.Responses;

namespace MyFinance.Application.UseCases.HealthChecks.Queries.GetHealthChecksReport;

internal sealed class GetHealthChecksReportHandler(HealthCheckService healthCheckService) :
    IQueryHandler<GetHealthChecksReportQuery, HealthyServicesResponse>
{
    private readonly HealthCheckService _healthCheckService = healthCheckService;

    public async Task<Result<HealthyServicesResponse>> Handle(GetHealthChecksReportQuery request, CancellationToken cancellationToken)
    {
        var report = await _healthCheckService.CheckHealthAsync(cancellationToken);
        var healthChecksReportResponse = HealthChecksMapper.DTR.Map(report);

        if (report.Status == HealthStatus.Unhealthy)
            return Result.Fail(new UnhealthyServicesError(report));

        return Result.Ok(healthChecksReportResponse);
    }
}
